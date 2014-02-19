using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

namespace Kwyjibo.ML.Features
{
    /// <summary>
    /// A static utility class that contains methods for extracting features from Bitmaps.
    /// </summary>
    /// <remarks></remarks>
    public static class BitmapFeatures
    {
        /// <summary>
        /// Calculates the gray-scale intensities of the pixels of the specified BMP.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns>A 2D array of doubles representing the gray-scale intensities of the pixels of the specified BMP</returns>
        /// <remarks></remarks>
        public static unsafe double[,] Intensity(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            double[,] features = new double[bmp.Width, bmp.Height];

            byte* bmpPtr = (byte*)(data.Scan0);
            int move = data.Stride - data.Width * 3;

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    features[j, i] = (bmpPtr[2] + bmpPtr[1] + bmpPtr[0]) / 3;
                    bmpPtr += 3;
                }
                bmpPtr += move;
            }

            bmp.UnlockBits(data);

            return features;
        }
    }
}
