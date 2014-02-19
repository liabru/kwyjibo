using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kwyjibo.ML.Classifiers;

namespace Kwyjibo.ML.Instances
{

    /// <summary>
    /// Represents an object as set of numeric features and corresponding classification, that is either used as a training example or as an unseen object to be classified.
    /// </summary>
    /// <remarks></remarks>
    public class Instance
    {
        /// <summary>
        /// The classification of this instance.
        /// </summary>
        private string _class;

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The class.</value>
        /// <remarks></remarks>
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// The array of numeric features.
        /// </summary>
        private double[] features;

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>The features.</value>
        /// <remarks></remarks>
        public double[] Features
        {
            get { return features; }
            set { features = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Double"/> at the specified index.
        /// </summary>
        /// <remarks></remarks>
        public double this[int index]
        {
            get
            {
                return Features[index];
            }
            set
            {
                Features[index] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public Instance()
        {
            _class = "?";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Instance"/> class.
        /// </summary>
        /// <param name="class_">The class_.</param>
        /// <remarks></remarks>
        public Instance(string class_)
        {
            _class = class_;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Instance"/> class.
        /// </summary>
        /// <param name="class_">The class_.</param>
        /// <param name="length">The length.</param>
        /// <remarks></remarks>
        public Instance(string class_, int length)
        {
            _class = class_;
            features = new double[length];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Instance"/> class.
        /// </summary>
        /// <param name="class_">The class_.</param>
        /// <param name="features">The features.</param>
        /// <remarks></remarks>
        public Instance(string class_, double[] features)
        {
            this.features = features;
            _class = class_;
        }

        /// <summary>
        /// Performs the vector dot-product of this instance's features and a specified instance's.
        /// </summary>
        /// <param name="vector">The instance to dot with.</param>
        /// <returns>The dot product.</returns>
        /// <remarks></remarks>
        public double DotProduct(Instance vector)
        {
            double sum = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += vector.Features[i] * Features[i];
            }
            return sum;
        }

        /// <summary>
        /// Returns the pythagorean length of the feature vector.
        /// </summary>
        /// <returns>The length of the feature vector.</returns>
        /// <remarks></remarks>
        public double Length()
        {
            double sum = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += Features[i] * Features[i];
            }
            return Math.Sqrt(sum);
        }

        /// <summary>
        /// Calculates the average (mean) of this instances features.
        /// </summary>
        /// <returns>The mean of the features.</returns>
        /// <remarks></remarks>
        public double Mean()
        {
            double sum = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += Features[i];
            }
            return sum / Features.Length;
        }

        /// <summary>
        /// Calculates the standard deviation of this instances features.
        /// </summary>
        /// <returns>The standard deviation of this instances features</returns>
        /// <remarks></remarks>
        public double StandardDeviation()
        {
            double sum = 0.0001f, mean = Mean();
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += (Features[i] - mean) * (Features[i] - mean);
            }
            return Math.Sqrt(sum / Features.Length);
        }

        /// <summary>
        /// Calculates the squared Euclidean distance of this instance to a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The squared distance.</returns>
        /// <remarks></remarks>
        public double DistanceSquared(Instance vector)
        {
            double sum = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += (vector.Features[i] - Features[i])
                        * (vector.Features[i] - Features[i]);
            }
            return sum;
        }

        /// <summary>
        /// Calculates the Euclidean distance of this instance to a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The distance.</returns>
        /// <remarks></remarks>
        public double EuclideanDistance(Instance vector)
        {
            return Math.Sqrt(DistanceSquared(vector));
        }

        /// <summary>
        /// Calculates the Manhattan distance of this instance to a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The Manhattan distance.</returns>
        /// <remarks></remarks>
        public double ManhattanDistance(Instance vector)
        {
            double sum = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += Math.Abs(vector.Features[i] - Features[i]);
            }
            return sum;
        }

        /// <summary>
        /// Calculates the Chebyshev distance of this instance to a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The chebyshev distance.</returns>
        /// <remarks></remarks>
        public double ChebyshevDistance(Instance vector)
        {
            double max = double.NegativeInfinity, dist = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                dist = Math.Abs(vector.Features[i] - Features[i]);
                if (dist > max) max = dist;
            }
            return dist;
        }

        /// <summary>
        /// Calculates the Minkowski distance of this instance to a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <param name="power">The Minkowski power.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public double MinkowskiDistance(Instance vector, double power)
        {
            double sum = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += Math.Pow(Math.Abs(vector.Features[i] - Features[i]), power);
            }
            return Math.Pow(sum, 1 / power);
        }

        /// <summary>
        /// Calculates the Cosine similarity of this instance with a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The Cosine similarity.</returns>
        /// <remarks></remarks>
        public double CosineSimilarity(Instance vector)
        {
            return DotProduct(vector) / (Length() * vector.Length());
        }

        /// <summary>
        /// Calculates the Pearson similarity of this instance with a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The Pearson similarity.</returns>
        /// <remarks></remarks>
        public double PearsonSimilarity(Instance vector)
        {
            double meanx = Mean(), meany = vector.Mean();
            double sdx = StandardDeviation(), sdy = vector.StandardDeviation();
            double sum = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                sum += ((Features[i] - meanx) / sdx) * ((vector.Features[i] - meany) / sdy);
            }
            return sum / Features.Length;
        }

        /// <summary>
        /// Calculates the Jaccard similarity of this instance with a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The Jaccard similarity.</returns>
        /// <remarks></remarks>
        public double JaccardSimilarity(Instance vector)
        {
            double m11 = 0;
            for (int i = 0; i < Features.Length; i += 1)
            {
                m11 += (Features[i] == vector.Features[i]) ? 1 : 0;
            }
            return m11 / Features.Length;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            string s = "<";
            foreach (double f in Features) s += f + ", ";
            return s + ">";
        }
    }
}
