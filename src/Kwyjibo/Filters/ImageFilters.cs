using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Kwyjibo
{
    /// <summary>
    /// Static utility class that provides various Bitmap image filters and tools. All methods are applied in-place.
    /// </summary>
    /// <remarks></remarks>
    public static class ImageFilters
    {

        /// <summary>
        /// Performs HSL filtering, with under and over tolerance. If a pixel is out of range, it is set to black, otherwise it is unaltered.
        /// </summary>
        /// <param name="bmp">The image to filter.</param>
        /// <param name="h">The target hue.</param>
        /// <param name="s">The target saturation.</param>
        /// <param name="l">The target lightness.</param>
        /// <param name="ht">The hue tolerance.</param>
        /// <param name="st">The saturation tolerance.</param>
        /// <param name="lt">The lightness tolerance.</param>
        /// <remarks></remarks>
        public static unsafe void HSLFilter(Bitmap bmp, float h, float s, float l, float ht, float st, float lt)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* bmpPtrA = (byte*)(data.Scan0);
            int move = data.Stride - data.Width * 3;

            float hd, sd, ld;
            Color c;

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    c = Color.FromArgb(bmpPtrA[2], bmpPtrA[1], bmpPtrA[0]);
                    hd = Math.Abs(((((c.GetHue() - h) % 360) + 540) % 360) - 180);
                    sd = Math.Abs(c.GetSaturation() - s);
                    ld = Math.Abs(c.GetBrightness() - l);

                    if (hd < 0 || hd > ht || sd > st || ld > lt)
                    {
                        bmpPtrA[0] = 0;
                        bmpPtrA[1] = 0;
                        bmpPtrA[2] = 0;
                    }

                    bmpPtrA += 3;
                }
                bmpPtrA += move;
            }

            bmp.UnlockBits(data);
        }

        /// <summary>
        /// Attempts to even out lighting differences in the image.
        /// </summary>
        /// <param name="bmp">The image to filter.</param>
        /// <remarks></remarks>
        public static unsafe void FlattenLighting(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* bmpPtrA = (byte*)(data.Scan0);
            int move = data.Stride - data.Width * 3;

            byte B;

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    B = (byte)(255 - (bmpPtrA[2] * 0.21f + bmpPtrA[1] * 0.71f + bmpPtrA[0] * 0.007f));

                    bmpPtrA[0] = OverlayBlend(B, bmpPtrA[0]);
                    bmpPtrA[1] = OverlayBlend(B, bmpPtrA[1]);
                    bmpPtrA[2] = OverlayBlend(B, bmpPtrA[2]);
                    bmpPtrA += 3;
                }
                bmpPtrA += move;
            }

            bmp.UnlockBits(data);
        }

        /// <summary>
        /// Converts the Bitmap to grayscale, but does not change the pixel format.
        /// </summary>
        /// <param name="bmp">The image to filter.</param>
        /// <remarks></remarks>
        public static unsafe void Grayscale(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* bmpPtrA = (byte*)(data.Scan0);
            int move = data.Stride - data.Width * 3;

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    bmpPtrA[0] = bmpPtrA[1] = bmpPtrA[2] = (byte)(bmpPtrA[2] * 0.3f + bmpPtrA[1] * 0.59f + bmpPtrA[0] * 0.11f);

                    bmpPtrA += 3;
                }
                bmpPtrA += move;
            }

            bmp.UnlockBits(data);
        }

        /// <summary>
        /// Overlay blend mode.
        /// </summary>
        /// <param name="B">The B.</param>
        /// <param name="L">The L.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static byte OverlayBlend(byte B, byte L)
        {
            return (byte)((L < 128) ? (2 * B * L / 255) : (255 - 2 * (255 - B) * (255 - L) / 255));
        }

        /// <summary>
        /// Multiply blend mode.
        /// </summary>
        /// <param name="B">The B.</param>
        /// <param name="L">The L.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static byte MultiplyBlend(byte B, byte L)
        {
            return (byte)((B * L) / 255);
        }

        /// <summary>
        /// Thresholds the specified image.
        /// </summary>
        /// <param name="bmp">The image to filter.</param>
        /// <param name="threshold">The theshold value.</param>
        /// <remarks></remarks>
        public static unsafe void Threshold(Bitmap bmp, int threshold)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* bmpPtrA = (byte*)(data.Scan0);
            int move = data.Stride - data.Width * 3;

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    bmpPtrA[0] = bmpPtrA[1] = bmpPtrA[2] = (byte)(bmpPtrA[2] + bmpPtrA[1] + bmpPtrA[0] > threshold ? 0 : 255);

                    bmpPtrA += 3;
                }
                bmpPtrA += move;
            }

            bmp.UnlockBits(data);
        }

        /// <summary>
        /// Masks out any pixels of the source image that are black in the mask image.
        /// </summary>
        /// <param name="bmp">The image to be masked.</param>
        /// <param name="bmp2">The mask.</param>
        /// <remarks></remarks>
        public static unsafe void Mask(Bitmap bmp, Bitmap mask)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData data2 = mask.LockBits(new Rectangle(0, 0, mask.Width, mask.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* bmpPtrA = (byte*)(data.Scan0);
            byte* bmpPtrB = (byte*)(data2.Scan0);
            int move = data.Stride - data.Width * 3;

            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++)
                {
                    if (bmpPtrB[0] == 0) bmpPtrA[0] = bmpPtrA[1] = bmpPtrA[2] = 0;

                    bmpPtrA += 3;
                    bmpPtrB += 3;
                }
                bmpPtrA += move;
                bmpPtrB += move;
            }

            bmp.UnlockBits(data);
            mask.UnlockBits(data2);
        }

        /// <summary>
        /// Takes a 5-point multi-sample average at the given bitmap position
        /// </summary>
        /// <param name="bmp">The image to sample from.</param>
        /// <param name="x">The x position to sample.</param>
        /// <param name="y">The x position to sample.</param>
        /// <remarks></remarks>
        public static HSBColor SampleHSBAverage(Bitmap bmp, int x, int y)
        {
            var samples = new Color[5];

            // sample 5 pixels around x, y
            samples[0] = bmp.GetPixel(x, y);
            samples[1] = bmp.GetPixel(x - 2, y);
            samples[2] = bmp.GetPixel(x, y - 2);
            samples[3] = bmp.GetPixel(x + 2, y);
            samples[4] = bmp.GetPixel(x, y + 2);

            // take the average HSB
            var hue = samples[0].GetHue() + samples[1].GetHue() + samples[2].GetHue() + samples[3].GetHue() + samples[4].GetHue();
            var sat = samples[0].GetSaturation() + samples[1].GetSaturation() + samples[2].GetSaturation() + samples[3].GetSaturation() + samples[4].GetSaturation();
            var bri = samples[0].GetBrightness() + samples[1].GetBrightness() + samples[2].GetBrightness() + samples[3].GetBrightness() + samples[4].GetBrightness();

            return new HSBColor
            {
                Hue = (decimal)hue / 5m,
                Saturation = (decimal)sat / 5m,
                Brightness = (decimal)bri / 5m
            };
        }
    }

    public struct HSBColor
    {
        public decimal Hue;
        public decimal Saturation;
        public decimal Brightness;
    }
}
