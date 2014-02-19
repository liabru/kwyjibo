using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Speech.Synthesis;
using System.IO;
using Kwyjibo.ML.Classifiers;
using Kwyjibo.ML.Instances;

namespace Kwyjibo.Modules
{
    /// <summary>
    /// This class implements the highest level Kwyjibo module. It manages and controls instances of every other module in the pipeline.
    /// </summary>
    /// <remarks></remarks>
    public class KwyjiboController
    {
        public VideoInput Video { get; set; }
        public TileDetector TileDetector { get; set; }
        public BoardDetector BoardDetector { get; set; }
        public TileOCR TileOcr { get; set; }
        public ScrabbleGame Game { get; set; }
        public KNearestClassifier WordClassifier { get; set; }
        public List<Word> NewWords { get; set; }

        public KwyjiboController(string trainingPath, string wordsPath, string wordProbPath)
        {
            // create our instances of each module
            Video = new VideoInput();
            TileDetector = new TileDetector();
            BoardDetector = new BoardDetector();
            TileOcr = new TileOCR(trainingPath);
            Game = new ScrabbleGame();

            // keep state of the words we find after Process
            NewWords = new List<Word>();

            // intialise the word classifier using supplied word list and word probability files
            setupWordClassifier(wordsPath, wordProbPath);
        }

        /*
         *
         *  The pipeline
         * 
         *  This big ugly Process method takes a webcam frame and pushes it through the module pipeline
         *  The module instances each process the frame, updating their own internal state in real-time and in-place
         * 
         *  HINDSIGHT: a functional, stateless approach may have been better? (but probably more verbose)
         * 
         *  TODO: make this less ugly
         * 
         */

        public void Process(Bitmap frame, bool tileDetectionEnabled, bool pauseRealtime, bool tileRecognitionEnabled, 
                            bool drawOcrResults, bool drawTileRegions, bool drawTileExtractions, 
                            bool placeDetectedTiles, bool drawBoardRegion, bool boardDetectionEnabled)
        {
            Graphics graphics;

            // Flatten lighting if enabled
            if (BoardDetector.FlattenLighting)
                ImageFilters.FlattenLighting(frame);

            // Board detection if enabled
            if (boardDetectionEnabled) 
                BoardDetector.Process(frame);

            // Board rectification
            BoardDetector.Rectify(frame);

            // Tile detection
            TileDetector.Process(BoardDetector.RectifiedBoard);

            // Display the board region overlay if enabled
            if (BoardDetector.HasDetected && drawBoardRegion)
            {
                graphics = Graphics.FromImage(frame);
                PointF[] corners = new PointF[4];
                var padding = 3;

                for (int i = 0; i < 4; i++)
                {
                    var xx = (i == 0 || i == 3) ? -padding : padding;
                    var yy = (i == 0 || i == 1) ? -padding : padding;
                    corners[i] = new PointF(BoardDetector.BoardCorners[i].X + xx, BoardDetector.BoardCorners[i].Y + yy);
                }

                graphics.DrawPolygon(new Pen(Color.Black, 6f), corners);
                graphics.DrawPolygon(new Pen(Color.Red, 4f), corners);
                graphics.Dispose();
            }

            // Clear the unplaced previous frame's tile detections
            Game.Board.ClearUnplacedCells();

            // Get a graphics context for the board image after rectification
            graphics = Graphics.FromImage(BoardDetector.RectifiedBoard);

            // For every detected tile
            foreach (Blob blob in TileDetector.TileBlobs)
            {
                // Upscale the detected tile region but make it slightly larger
                var border = 5;
                var br = blob.Rectangle;
                var x = Math.Max(0, -border + br.X * TileDetector.Scale);
                var y = Math.Max(0, -border + br.Y * TileDetector.Scale);
                var width = Math.Min(BoardDetector.RectifiedBoard.Width, border + br.Width * TileDetector.Scale);
                var height = Math.Min(BoardDetector.RectifiedBoard.Height, border + br.Height * TileDetector.Scale);
                var region = new Rectangle(x, y, width, height);

                // Search for tiles if enabled
                if (tileDetectionEnabled)
                {
                    try
                    {
                        // Get a copy of the rectified board image
                        Bitmap extract = BoardDetector.RectifiedBoard.Clone(region, PixelFormat.Format24bppRgb);

                        // Extract the current blob's image
                        TileDetector.BlobCounter.ExtractBlobsImage(TileDetector.FilteredBoard, blob, false);
                        Bitmap blobImage = blob.Image.ToManagedImage();

                        // Resize the blob image
                        Bitmap rectifiedBlobImage = (new ResizeNearestNeighbor(extract.Width, extract.Height)).Apply(blobImage);

                        // Apply the tile mask
                        ImageFilters.Mask(extract, rectifiedBlobImage);

                        // Perform letter blob extraction (and recognition if enabled)
                        if (pauseRealtime) 
                            TileOcr.Recognise = true;
                        TileOcr.Process(extract);
                        TileOcr.Recognise = tileRecognitionEnabled;

                        // For every OCR result (recognised or not)
                        foreach (OCRResult result in TileOcr.Results)
                        {
                            // Get the absolute blob position
                            var xx = region.X + result.Blob.Rectangle.X;
                            var yy = region.Y + result.Blob.Rectangle.Y;

                            // Display OCR result overlay if enabled
                            if (drawOcrResults || pauseRealtime)
                            {
                                graphics.FillRectangle(new SolidBrush(Color.DarkGray), xx, yy, 32, 32);
                                graphics.DrawString(result.Letter, new Font("Verdana", 20, FontStyle.Bold), new SolidBrush(Color.Black), xx + 1, yy + 1);
                                graphics.DrawString(result.Letter, new Font("Verdana", 20, FontStyle.Bold), new SolidBrush(Color.White), xx, yy);
                            }

                            // Display tile region if enabled
                            if (drawTileRegions) 
                                graphics.DrawRectangle(new Pen(Color.Red, 2),              
                                    xx - 5, yy - 5, result.Blob.Rectangle.Width + 10, result.Blob.Rectangle.Height + 10);

                            // Place the tile on to the virtual Scrabble board if required
                            if (placeDetectedTiles)
                                Game.Board.PlaceTile(Game.CurrentPlayer, result.Letter, xx + 6, yy + 6,
                                                     BoardDetector.RectifiedBoard.Width, BoardDetector.RectifiedBoard.Height, result.Blob.Rectangle);

                            // Display the raw tile extractions if enabled
                            if (drawTileExtractions) 
                                graphics.DrawImageUnscaled(result.Image, new Point(xx, yy));
                        }
                    }
                    catch
                    {
                        // sometimes this may fail :(
                    }
                }
            }

            graphics.Dispose();
        }

        private void setupWordClassifier(string wordsPath, string wordProbPath)
        {
            Game.ValidWords = new WordDict(wordsPath);

            WordClassifier = new KNearestClassifier(1, Metric.WeightedHammingDistance, WeightMode.Modal);
            WordClassifier.TrainingInstances = new List<Instance>();
            string[] lines = File.ReadAllLines(wordProbPath);

            foreach (string line in lines)
            {
                string[] word = line.Split(' ');
                InstanceS x = new InstanceS(word[0]);
                x.Weight = float.Parse(word[1]);
                WordClassifier.TrainingInstances.Add(x);
            }
        }
    }
}