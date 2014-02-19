using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kwyjibo.ML.Features
{
    /// <summary>
    /// A static utility class that provides feature reduction routines.
    /// </summary>
    /// <remarks></remarks>
	public static class FeatureReduction
	{
        /// <summary>
        /// Sorts the specified features.
        /// </summary>
        /// <param name="features">The features.</param>
        /// <returns>A sorted array of features.</returns>
        /// <remarks></remarks>
        public static double[] Sort(double[] features)
        {
            Array.Sort(features);
            return features;
        }

        /// <summary>
        /// Performs feature reduction using the merge algorithm.
        /// </summary>
        /// <param name="features">The features.</param>
        /// <param name="newLength">The desired length.</param>
        /// <returns>The feature array after reduction.</returns>
        /// <remarks></remarks>
        public static double[] Merge(double[] features, int newLength)
        {
            if (features.Length == newLength) 
                return features;

            double[] merged = new double[newLength];

            for (int i = 0; i < features.Length; i++)
            {
                double j = ((double)i / (double)(features.Length - 1)) * (newLength - 1);
                merged[(int)j] += features[i];
            }

            return merged;
        }

        /// <summary>
        /// Discretizes the specified features.
        /// </summary>
        /// <param name="features">The features.</param>
        /// <param name="n">The desired number of discrete levels.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double[] Discretize(double[] features, int n)
        {
            double[] discretized = new double[features.Length];
            double max = double.MinValue;

            for (int i = 0; i < features.Length; i++)
            {
                if (features[i] > max) max = features[i];
            }

            for (int i = 0; i < features.Length; i++)
            {
                discretized[i] = Math.Round((features[i] / max) * n);
            }

            return discretized;
        }

        /// <summary>
        /// Flattens a 2D array of features into a 1D array, row by row.
        /// </summary>
        /// <param name="features">The features.</param>
        /// <returns>A flat 1D representation of the specified 2D feature array.</returns>
        /// <remarks></remarks>
        public static double[] Flatten(double[,] features)
        {
            int width = features.GetUpperBound(1) + 1;
            int height = features.GetUpperBound(0) + 1;
            double[] flattened = new double[width * height];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    flattened[(i * width) + j] = features[i, j];
                }
            }

            return flattened;
        }

        /// <summary>
        /// Performs feature reduction using the grid merge algorithm.
        /// </summary>
        /// <param name="features">The features.</param>
        /// <param name="length">The desired length.</param>
        /// <returns>The feature array after reduction.</returns>
        /// <remarks></remarks>
        public static double[] GridwiseMerge(double[,] features, int length)
        {
            double width = features.GetUpperBound(1) + 1;
            double height = features.GetUpperBound(0) + 1;

            if ((int)(height * width) == length) 
                return Flatten(features);

            double[] merged = new double[length];

            int s = (int)Math.Sqrt(length);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int x = (int)Math.Floor(((double)i / width) * s);
                    int y = (int)Math.Floor(((double)j / height) * s);
                    merged[(x * s) + y] += features[i, j];
                }
            }

            return merged;
        }

        /// <summary>
        /// Performs feature reduction using the row and column sum algorithm.
        /// </summary>
        /// <param name="features">The features.</param>
        /// <returns>The feature array after reduction.</returns>
        /// <remarks></remarks>
        public static double[] RowColSums(double[,] features)
        {
            int width = features.GetUpperBound(0) + 1;
            int height = features.GetUpperBound(1) + 1;
            double[] sums = new double[width + height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    sums[width + j] += features[i, j];
                    sums[i] += features[i, j];
                }
            }

            return sums;
        }
    }
}
