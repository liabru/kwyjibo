using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;

namespace Kwyjibo
{

    /// <summary>
    /// This class implements Scrabble board detection, extraction and rectification. This module is stateful.
    /// </summary>
    /// <remarks></remarks>
    public class BoardDetector
    {
        /// <summary>
        /// Resize filter.
        /// </summary>
        private ResizeBilinear resize;

        /// <summary>
        /// Median filter.
        /// </summary>
        private Median median;

        /// <summary>
        /// Quadrilateral finder filter.
        /// </summary>
        private QuadrilateralFinder quadFinder;

        /// <summary>
        /// Target HSL values for the colour filter.
        /// </summary>
        private float targetHue = 342f, targetSat = 0.7f, targetBri = 0.6f;

        /// <summary>
        /// Tolerance values for the colour filter.
        /// </summary>
        private float hueTol = 10f, satTol = 0.2f, briTol = 0.2f;

        /// <summary>
        /// List of N detected board corners from previous frames, used for smoothing corners output.
        /// </summary>
        private List<List<IntPoint>> oldCorners;

        /// <summary>
        /// Contains four points, one for each detected board corner.
        /// </summary>
        private List<IntPoint> boardCorners;

        /// <summary>
        /// Gets or sets the board corners.
        /// </summary>
        /// <value>The board corners.</value>
        /// <remarks></remarks>
        public List<IntPoint> BoardCorners
        {
            get { return boardCorners; }
            set { boardCorners = value; }
        }

        /// <summary>
        /// The number of sets of corners to store and average when smoothing.
        /// </summary>
        private int n = 3;

        /// <summary>
        /// Gets or sets the number of sets of corners to store and average when smoothing.
        /// </summary>
        /// <value>The N.</value>
        /// <remarks></remarks>
        public int N
        {
            get { return n; }
            set { n = value; }
        }

        /// <summary>
        /// The resolution that board detection should be performed at (result is 1/scale).
        /// </summary>
        private int scale = 4;

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
        /// Stores the rectified board image.
        /// </summary>
        public Bitmap rectifiedBoard;

        /// <summary>
        /// Gets or sets the rectified board.
        /// </summary>
        /// <value>The rectified board.</value>
        /// <remarks></remarks>
        public Bitmap RectifiedBoard
        {
            get { return rectifiedBoard; }
            set { rectifiedBoard = value; }
        }

        /// <summary>
        /// Stores the board image after colour filtering.
        /// </summary>
        public Bitmap filteredBoard;

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
        /// Stores whether the detector has detected the board.
        /// </summary>
        private bool hasDetected = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance board is detected.
        /// </summary>
        /// <value><c>true</c> if this instance has detected; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool HasDetected
        {
            get { return hasDetected; }
            set { hasDetected = value; }
        }

        /// <summary>
        /// Toggles image enhancements (lighting flattening).
        /// </summary>
        private bool flattenLighting = false;

        /// <summary>
        /// Gets or sets image enhancements (lighting flattening).
        /// </summary>
        /// <value><c>true</c> if enhance; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool FlattenLighting
        {
            get { return flattenLighting; }
            set { flattenLighting = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public BoardDetector()
        {
            this.median = new Median();
            this.quadFinder = new QuadrilateralFinder();
            this.resize = new ResizeBilinear(640, 480);
            this.boardCorners = new List<IntPoint>();
            this.oldCorners = new List<List<IntPoint>>();
            this.filteredBoard = new Bitmap(640, 480);
            this.rectifiedBoard = new Bitmap(640, 480);
        }

        /// <summary>
        /// Sets the board colour to detect.
        /// </summary>
        /// <param name="hue">The hue.</param>
        /// <param name="sat">The saturation.</param>
        /// <param name="bri">The brightness.</param>
        /// <remarks></remarks>
        public void SetColour(float hue, float sat, float bri)
        {
            targetHue = hue;
            targetSat = sat;
            targetBri = bri;
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
        /// Updates the list of stored corners.
        /// </summary>
        /// <param name="newCorners">The new corners to add.</param>
        /// <remarks></remarks>
        private void UpdateCorners(List<IntPoint> newCorners)
        {
            for (int i = 0; i < 4; i++)
            {
                newCorners[i] = new IntPoint(newCorners[i].X * Scale, newCorners[i].Y * Scale);
            }
            
            oldCorners.Add(newCorners);
            if (oldCorners.Count > N) oldCorners.RemoveAt(0);

            BoardCorners = new List<IntPoint>();
            BoardCorners.Add(new IntPoint(0, 0)); BoardCorners.Add(new IntPoint(0, 0));
            BoardCorners.Add(new IntPoint(0, 0)); BoardCorners.Add(new IntPoint(0, 0));
            
            foreach (List<IntPoint> cs in oldCorners)
            {
                for (int j = 0; j < 4; j += 1)
                {
                    BoardCorners[j] += cs[j];
                }
            }

            for (int j = 0; j < 4; j += 1)
            {
                BoardCorners[j] /= oldCorners.Count;
            }
            

            HasDetected = ValidCorners(BoardCorners);
        }

        /// <summary>
        /// Rectifies the specified source image, using the smoothed board corners and quadrilateral transformation.
        /// </summary>
        /// <param name="srcImage">The source image.</param>
        /// <remarks></remarks>
        public void Rectify(Bitmap srcImage)
        {
            try
            {
                int size = 480;
                QuadrilateralTransformation qt = new QuadrilateralTransformation(BoardCorners, size, size);
                RectifiedBoard = qt.Apply(srcImage);
            }
            catch { RectifiedBoard = srcImage; }
        }

        /// <summary>
        /// Gets the optimal size.
        /// </summary>
        /// <param name="cs">The corners.</param>
        /// <returns>The optimal size for rectification.</returns>
        /// <remarks></remarks>
        private int GetSize(List<IntPoint> cs)
        {
            float x1 = cs[0].X, y1 = cs[0].Y;
            float x2 = cs[1].X, y2 = cs[1].Y;
            float x3 = cs[2].X, y3 = cs[2].Y;
            float x4 = cs[3].X, y4 = cs[3].Y;
            float area = 0.5f * ((y2 - y4) * x1 + (y3 - y1) * x2 + (y4 - y2) * x3 + (y1 - y3) * x4);
            return (int)Math.Round(Math.Sqrt(area));
        }

        /// <summary>
        /// Sanity checks the corners form a possible board.
        /// </summary>
        /// <param name="cs">The corners.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        private bool ValidCorners(List<IntPoint> cs)
        {
            if (cs.Count != 4) return false;

            for (int j = 0; j < 4; j++)
            {
                int k = (j + 1) % 4;
                int l = (j + 2) % 4;
                Double angle = Math.Abs(Math.Atan2(cs[k].Y - cs[j].Y, cs[k].X - cs[j].X) - Math.Atan2(cs[l].Y - cs[k].Y, cs[l].X - cs[k].X)) % Math.PI;
                if (angle < 0.8 || angle > 2.3) return false;
            }

            return true;
        }

        /// <summary>
        /// Clamps the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="max">The max.</param>
        /// <param name="min">The min.</param>
        /// <returns>The value, clamped between min and max.</returns>
        /// <remarks></remarks>
        private T Clamp<T>(T value, T max, T min) where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }

        /// <summary>
        /// Enlarges the corners.
        /// </summary>
        /// <param name="cs">The cs.</param>
        /// <remarks></remarks>
        private void EnlargeCorners(List<IntPoint> cs)
        {
            int k = 0;
            cs[0] = new IntPoint(Clamp(cs[0].X - k, 640, 0), Clamp(cs[0].Y + k, 480, 0));
            cs[1] = new IntPoint(Clamp(cs[1].X - k, 640, 0), Clamp(cs[1].Y - k, 480, 0));
            cs[2] = new IntPoint(Clamp(cs[2].X + k, 640, 0), Clamp(cs[2].Y - k, 480, 0));
            cs[3] = new IntPoint(Clamp(cs[3].X + k, 640, 0), Clamp(cs[3].Y + k, 480, 0));
        }

        /// <summary>
        /// Sorts the corners.
        /// </summary>
        /// <param name="cors">The cors.</param>
        /// <remarks></remarks>
        private void SortCorners(List<IntPoint> cors)
        {
            float cx = 0, cy = 0;
            for (int j = 0; j < 4; j++)
            {
                cx += cors[j].X;
                cy += cors[j].Y;
            }
            cx /= 4;
            cy /= 4;

            cors.Sort(delegate(IntPoint a, IntPoint b)
            {
                return (Math.Atan2(a.X - cx, a.Y - cy) < Math.Atan2(b.X - cx, b.Y - cy)) ? 1 : -1;
            });

            List<IntPoint> cors2 = new List<IntPoint>();
            cors2.Add(cors[3]); cors2.Add(cors[0]);
            cors2.Add(cors[1]); cors2.Add(cors[2]);
            cors.Clear();
            cors.AddRange(cors2);
        }

        /// <summary>
        /// Processes the specified SRC image.
        /// </summary>
        /// <param name="srcImage">The SRC image.</param>
        /// <remarks></remarks>
        public void Process(Bitmap srcImage)
        {
            resize.NewWidth = srcImage.Width / Scale;
            resize.NewHeight = srcImage.Height / Scale;

            FilteredBoard = resize.Apply(srcImage);

            ImageFilters.HSLFilter(FilteredBoard, targetHue, targetSat, targetBri, hueTol, satTol, briTol);

            median.ApplyInPlace(FilteredBoard);

            GaussianBlur blr = new GaussianBlur(1, 2);
            blr.ApplyInPlace(FilteredBoard);

            List<IntPoint> cors = new List<IntPoint>();
            try
            {
                cors = quadFinder.ProcessImage(FilteredBoard);
            }
            catch { }

            if (ValidCorners(cors))
            {
                EnlargeCorners(cors);
                SortCorners(cors);
                UpdateCorners(cors);
            }
        }

    }
}
