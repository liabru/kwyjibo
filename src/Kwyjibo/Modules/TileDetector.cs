using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;

namespace Kwyjibo
{
    /// <summary>
    /// This class implements Scrabble tile detection and extraction. This module is stateful.
    /// </summary>
    /// <remarks></remarks>
    public class TileDetector
    {
        /// <summary>
        /// Target HSL values for the colour filter.
        /// </summary>
        private float targetHue = 35f, targetSat = 1f, targetBri = 0.9f;

        /// <summary>
        /// Tolerance values for the colour filter.
        /// </summary>
        private float hueTol = 30f, satTol = 0.7f, briTol = 0.7f;

        /// <summary>
        /// The list of blobs that were extracted in the last processed frame.
        /// </summary>
        private List<Blob> tileBlobs;

        /// <summary>
        /// Gets or sets the tile blobs.
        /// </summary>
        /// <value>The tile blobs.</value>
        /// <remarks></remarks>
        public List<Blob> TileBlobs
        {
            get { return tileBlobs; }
            set { tileBlobs = value; }
        }

        /// <summary>
        /// Toggles image enhancements (lighting flattening).
        /// </summary>
        private bool enhance = false;

        /// <summary>
        /// Gets or sets image enhancements (lighting flattening).
        /// </summary>
        /// <value><c>true</c> if enhance; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Enhance
        {
            get { return enhance; }
            set { enhance = value; }
        }

        /// <summary>
        /// The resolution that board detection should be performed at (result is 1/scale).
        /// </summary>
        private int scale = 6;

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        /// <remarks></remarks>
        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// Resize filter.
        /// </summary>
        private ResizeBilinear resize;

        /// <summary>
        /// Median filter.
        /// </summary>
        private Median median;

        /// <summary>
        /// Blob counter.
        /// </summary>
        private BlobCounterBase blobCounter;

        /// <summary>
        /// Gets or sets the blob counter.
        /// </summary>
        /// <value>The blob counter.</value>
        /// <remarks></remarks>
        public BlobCounterBase BlobCounter
        {
            get { return blobCounter; }
            set { blobCounter = value; }
        }

        /// <summary>
        /// Stores the board image after colour filtering.
        /// </summary>
        private Bitmap filteredBoard;

        /// <summary>
        /// Gets or sets the filtered board.
        /// </summary>
        /// <value>The filtered board.</value>
        /// <remarks></remarks>
        public Bitmap FilteredBoard
        {
            get { return filteredBoard; }
            set { filteredBoard = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public TileDetector()
        {
            median = new Median();
            blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinWidth = (int)Math.Ceiling(15f / scale);
            blobCounter.MinHeight = (int)Math.Ceiling(15f / scale);
            blobCounter.ObjectsOrder = ObjectsOrder.XY;
            resize = new ResizeBilinear(640, 480);
            this.filteredBoard = new Bitmap(640, 480);
            this.tileBlobs = new List<Blob>();
        }

        /// <summary>
        /// Sets the tile colour to detect.
        /// </summary>
        /// <param name="hue">The hue.</param>
        /// <param name="sat">The saturation.</param>
        /// <param name="bri">The brightness.</param>
        /// <remarks></remarks>
        public void SetColour(float hue, float sat, float bri)
        {
            this.targetHue = hue;
            this.targetSat = sat;
            this.targetBri = bri;
        }

        /// <summary>
        /// Sets the colour tolerance.
        /// </summary>
        /// <param name="hueTol">The hue tolerance.</param>
        /// <param name="satTol">The sat tolerance.</param>
        /// <param name="briTol">The bri tolerance.</param>
        /// <remarks></remarks>
        public void SetTolerance(float hueTol, float satTol, float briTol)
        {
            this.hueTol = hueTol;
            this.satTol = satTol;
            this.briTol = briTol;
        }

        /// <summary>
        /// Processes the specified SRC image.
        /// </summary>
        /// <param name="srcImage">The SRC image.</param>
        /// <remarks></remarks>
        public void Process(Bitmap srcImage)
        {
            if (srcImage == null || srcImage.Width < 10 || srcImage.Height < 10) 
                return;

            resize.NewWidth = srcImage.Width / Scale;
            resize.NewHeight = srcImage.Height / Scale;

            FilteredBoard = resize.Apply(srcImage);

            if (Enhance) 
                ImageFilters.FlattenLighting(FilteredBoard);

            ImageFilters.HSLFilter(FilteredBoard, targetHue, targetSat, targetBri, hueTol, satTol, briTol);

            median.ApplyInPlace(FilteredBoard);
            GaussianBlur blr = new GaussianBlur(2, 2);
            blr.ApplyInPlace(FilteredBoard);

            TileBlobs.Clear();

            try
            {
                BlobCounter.ProcessImage(FilteredBoard);

                Blob[] blobs = BlobCounter.GetObjectsInformation();
                TileBlobs.Clear();

                foreach (Blob b in blobs)
                {
                    if (b.Area < 10) continue;
                    TileBlobs.Add(b);
                }
            }
            catch { }
        }

    }
}
