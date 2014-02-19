namespace AForge.Imaging.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using AForge.Imaging;

    class SauvolaAdaptiveThresholding : BaseInPlacePartialFilter
    {
        private const float DEFAULT_WeightingFactor = 0.3f;
        private const short DEFAULT_WindowSize = 40;

        private float weightingFactor;
        private short windowSize;
        // private format translation dictionary
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

        /// <summary>
        /// Format translations dictionary.
        /// </summary>
        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }
        public float WeightingFactor
        {
            get { return this.weightingFactor; }
            set { this.weightingFactor = value; }
        }
        public short WindowSize
        {
            get { return this.windowSize; }
            set { this.windowSize = value; }
        }
        public SauvolaAdaptiveThresholding() :
            this(DEFAULT_WeightingFactor, DEFAULT_WindowSize) { }

        public SauvolaAdaptiveThresholding(float _weightFact, short _wsize)
        {
            this.WeightingFactor = _weightFact;
            this.WindowSize = _wsize;
            // initialize format translation dictionary
            formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format16bppGrayScale] = PixelFormat.Format16bppGrayScale;
        }
        protected override unsafe void ProcessFilter(UnmanagedImage image, Rectangle rect)
        {
            int whalf = windowSize >> 1; //half of windowsize
            whalf = windowSize >> 1;

            byte* ptr = (byte*)image.ImageData.ToPointer(); //input         

            // Calculate the integral image, and integral of the squared image
            ulong[,] integral_image = new ulong[image.Width, image.Height];
            ulong[,] rowsum_image = new ulong[image.Width, image.Height];
            ulong[,] integral_sqimg = new ulong[image.Width, image.Height];
            ulong[,] rowsum_sqimg = new ulong[image.Width, image.Height];

            int xmin, ymin, xmax, ymax, index;
            double diagsum, idiagsum, diff, sqdiagsum, sqidiagsum, sqdiff, area;
            double mean, std, threshold;

            for (int j = 0; j < image.Height; j++)
            {
                rowsum_image[0, j] = (ulong)*(ptr + j);
                rowsum_sqimg[0, j] = rowsum_image[0, j] * rowsum_image[0, j];
            }
            for (int i = 1; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    index = j * image.Width + i;
                    rowsum_image[i, j] = rowsum_image[i - 1, j] + (ulong)*(ptr + index);
                    rowsum_sqimg[i, j] = rowsum_sqimg[i - 1, j] + (ulong)(*(ptr + index) * *(ptr + index));
                }
            }

            for (int i = 0; i < image.Width; i++)
            {
                integral_image[i, 0] = rowsum_image[i, 0];
                integral_sqimg[i, 0] = rowsum_sqimg[i, 0];
            }
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 1; j < image.Height; j++)
                {
                    integral_image[i, j] = integral_image[i, j - 1] + rowsum_image[i, j];
                    integral_sqimg[i, j] = integral_sqimg[i, j - 1] + rowsum_sqimg[i, j];
                }
            }
            //Calculate the mean and standard deviation using the integral image
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    xmin = Math.Max(0, i - whalf);//max(0, i - whalf);
                    ymin = Math.Max(0, j - whalf);
                    xmax = Math.Min(image.Width - 1, i + whalf);
                    ymax = Math.Min(image.Height - 1, j + whalf);
                    area = (xmax - xmin + 1) * (ymax - ymin + 1);

                    if (xmin == 0 && ymin == 0)
                    { // Point at origin
                        diff = integral_image[xmax, ymax];
                        sqdiff = integral_sqimg[xmax, ymax];
                    }
                    else if (xmin == 0 && ymin != 0)
                    { // first column
                        diff = integral_image[xmax, ymax] - integral_image[xmax, ymin - 1];
                        sqdiff = integral_sqimg[xmax, ymax] - integral_sqimg[xmax, ymin - 1];
                    }
                    else if (xmin != 0 && ymin == 0)
                    { // first row
                        diff = integral_image[xmax, ymax] - integral_image[xmin - 1, ymax];
                        sqdiff = integral_sqimg[xmax, ymax] - integral_sqimg[xmin - 1, ymax];
                    }
                    else
                    { // rest of the image
                        diagsum = integral_image[xmax, ymax] + integral_image[xmin - 1, ymin - 1];
                        idiagsum = integral_image[xmax, ymin - 1] + integral_image[xmin - 1, ymax];
                        diff = diagsum - idiagsum;
                        sqdiagsum = integral_sqimg[xmax, ymax] + integral_sqimg[xmin - 1, ymin - 1];
                        sqidiagsum = integral_sqimg[xmax, ymin - 1] + integral_sqimg[xmin - 1, ymax];
                        sqdiff = sqdiagsum - sqidiagsum;
                    }

                    mean = diff / area;
                    std = Math.Sqrt((sqdiff - diff * diff / area) / (area - 1));
                    threshold = mean * (1 + WeightingFactor * ((std / 128) - 1));
                    if ((double)*(ptr + (j * image.Width + i)) < threshold)//if (gray_image[i, j] < threshold)
                        *(ptr + (j * image.Width + i)) = 0;
                    else
                        *(ptr + (j * image.Width + i)) = 255;
                }
            }
        }
    }
}