using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;

using Kwyjibo.ML.Classifiers;
using Kwyjibo.ML.Features;
using Kwyjibo.ML.Instances;
using Kwyjibo.ML.Validation;
using System.IO;

namespace Kwyjibo
{
    /// <summary>
    /// Implements letter detection, extraction and recognition using k-Nearest-Neighbour classification. This module is stateful.
    /// </summary>
    /// <remarks></remarks>
    public class TileOCR
    {
        /// <summary>
        /// The letter classifier.
        /// </summary>
        private KNearestClassifier classifier;

        /// <summary>
        /// The letter training examples.
        /// </summary>
        private List<Instance> training;

        /// <summary>
        /// The results of the last letter recognition.
        /// </summary>
        private List<OCRResult> results;

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        /// <remarks></remarks>
        public List<OCRResult> Results
        {
            get { return results; }
            set { results = value; }
        }

        /// <summary>
        /// Toggles letter recognition.
        /// </summary>
        private bool recognise = true;

        /// <summary>
        /// Toggles letter recognition.
        /// </summary>
        /// <value><c>true</c> for enabling recognition; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Recognise
        {
            get { return recognise; }
            set { recognise = value; }
        }

        /// <summary>
        /// Grayscale filter.
        /// </summary>
        private Grayscale grayscale;

        /// <summary>
        /// Invert filter.
        /// </summary>
        private Invert invert;

        /// <summary>
        /// Resize filter.
        /// </summary>
        private ResizeNearestNeighbor resize;

        /// <summary>
        /// Flood fill filter.
        /// </summary>
        private PointedColorFloodFill floodFill;

        /// <summary>
        /// Blob counter.
        /// </summary>
        private BlobCounterBase blobCounter;

        /// <summary>
        /// Adaptive threshold filter.
        /// </summary>
        private BradleyLocalThresholding threshold;

        /// <summary>
        /// Dilation filter.
        /// </summary>
        private BinaryDilatation3x3 dilate;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public TileOCR(string trainingPath)
        {
            classifier = new KNearestClassifier(1, Metric.EuclideanDistance, WeightMode.InverseDistance);
            training = LoadInstancesFromBitmaps(trainingPath);
            
            classifier.Train(training);

            results = new List<OCRResult>();

            grayscale = new Grayscale(0, 0.85, 0.15);
            invert = new Invert();
            resize = new ResizeNearestNeighbor(32, 32);
            floodFill = new PointedColorFloodFill(Color.Black);
            dilate = new BinaryDilatation3x3();
            blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinWidth = 4;
            blobCounter.MinHeight = 14;
            blobCounter.MaxWidth = 30;
            blobCounter.MaxHeight = 30;
            blobCounter.ObjectsOrder = ObjectsOrder.XY;
            threshold = new BradleyLocalThresholding();
            threshold.PixelBrightnessDifferenceLimit = 0;
            //Threshold.WindowSize = 20;
            threshold.WindowSize = 24;
        }

        /// <summary>
        /// Loads the example instances from stored bitmaps.
        /// </summary>
        /// <param name="path">The path containing training examples.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<Instance> LoadInstancesFromBitmaps(string path)
        {
            List<Instance> instances = new List<Instance>();

            string[] files = Directory.GetFiles(path, "*.bmp", SearchOption.AllDirectories);

            foreach (string fp in files)
            {
                string name = Path.GetFileNameWithoutExtension(fp);
                name = name.Replace("letter_", "");
                Bitmap bmp = (Bitmap)Bitmap.FromFile(fp);
                double[] features = BitmapFeatureExtract(bmp);
                instances.Add(new Instance(name, features));
            }

            return instances;
        }

        /// <summary>
        /// Extracts intensity features from the bitmap, and uses grid merge feature reduction.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private double[] BitmapFeatureExtract(Bitmap bmp)
        {
            double[,] features2D = BitmapFeatures.Intensity(bmp);
            double[] features = FeatureReduction.GridwiseMerge(features2D, 64);
            return features;
        }

        /// <summary>
        /// Processes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <remarks></remarks>
        public void Process(Bitmap image)
        {
            //Graphics g = Graphics.FromImage(image);
            /*
            float area = image.Width * image.Height;
            float borderWidth = (float)Math.Max(3, Math.Round(0.25 * (area / 1024)));

            g.DrawRectangle(new Pen(Color.Black, borderWidth), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            */
            image = grayscale.Apply(image);

            //ContrastStretch filter = new ContrastStretch();
            //HistogramEqualization filter = new HistogramEqualization();
            //ContrastCorrection filter = new ContrastCorrection(0.5);
            //BrightnessCorrection filter = new BrightnessCorrection(-0.15);
            //GammaCorrection filter = new GammaCorrection(1.5);
            //filter.ApplyInPlace(image);

            threshold.ApplyInPlace(image);
            invert.ApplyInPlace(image);

            float sideAvg = (image.Width + image.Height) / 2;
            float borderWidth = (float)Math.Max(2, Math.Round(0.02 * sideAvg));

            Bitmap temp = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(temp);
            g.DrawImageUnscaled(image, 0, 0);
            g.DrawRectangle(new Pen(Color.White, borderWidth), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            image = (new Threshold()).Apply(grayscale.Apply(temp));

            floodFill.StartingPoint = new IntPoint(0, 0);
            floodFill.ApplyInPlace(image);

            blobCounter.ProcessImage(image);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            //Results = new List<OCRResult>();
            Results.Clear();

            foreach (Blob b in blobs)
            {
                int w = b.Rectangle.Width; 
                int h = b.Rectangle.Height;

                if (w > 16 && h > 16 && h < 24)
                {
                    w = 16;
                    h = 16;
                }
                else
                //{
                    if (!(w >= 3 && w <= 5 && h >= 14 && h <= 17 && b.Fullness > 0.6f))
                    {
                        //Bitmap test = image.Clone(new Rectangle(b.Rectangle.X, b.Rectangle.Y, w, h), PixelFormat.Format24bppRgb);
                        if (b.Area < 20 || b.Fullness < 0.2f || b.Fullness > 0.9f || h > 18) 
                            continue;
                    }
                //}

                //blobCounter.MinWidth = 3;
                //blobCounter.MinHeight = 10;

                //if (b.Area < 20 || b.Fullness > 0.9f) continue;

                Bitmap slice = image.Clone(new Rectangle(b.Rectangle.X, b.Rectangle.Y, w, h), PixelFormat.Format24bppRgb);
                slice = grayscale.Apply(slice);
                slice = resize.Apply(slice);

                dilate.ApplyInPlace(slice);

                if (Recognise)
                {
                    Result res = classifier.Classify(new Instance("?", BitmapFeatureExtract(slice)));
                    Results.Add(new OCRResult(b, slice, res.Class, res.Value.ToString()));
                }
                else
                {
                    Results.Add(new OCRResult(b, slice, "?", ""));
                }
            }
           
        }
    }

    /// <summary>
    /// Represents a single character recognition.
    /// </summary>
    /// <remarks></remarks>
    public struct OCRResult
    {
        /// <summary>
        /// The blob that was recognised.
        /// </summary>
        private Blob blob;

        /// <summary>
        /// Gets or sets the blob.
        /// </summary>
        /// <value>The blob.</value>
        /// <remarks></remarks>
        public Blob Blob
        {
            get { return blob; }
            set { blob = value; }
        }

        /// <summary>
        /// The image it was recognised in.
        /// </summary>
        private Bitmap image;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        /// <remarks></remarks>
        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// The letter that was recognised.
        /// </summary>
        private string letter;

        /// <summary>
        /// Gets or sets the letter.
        /// </summary>
        /// <value>The letter.</value>
        /// <remarks></remarks>
        public string Letter
        {
            get { return letter; }
            set { letter = value; }
        }

        /// <summary>
        /// Stores a debug log of any details the classifier noted.
        /// </summary>
        private string log;

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>The log.</value>
        /// <remarks></remarks>
        public string Log
        {
            get { return log; }
            set { log = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OCRResult"/> struct.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <param name="image">The image.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="log">The log.</param>
        /// <remarks></remarks>
        public OCRResult(Blob blob, Bitmap image, string letter, string log)
        {
            this.blob = blob;
            this.image = image;
            this.letter = letter;
            this.log = log;
        }
    }
}
