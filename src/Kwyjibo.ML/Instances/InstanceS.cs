using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kwyjibo.ML.Instances
{
    /// <summary>
    /// Represents an object as set of nominal (single character) features and corresponding classification, that is either used as a training example or as an unseen object to be classified.
    /// </summary>
    /// <remarks></remarks>
    public class InstanceS : Instance
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
        public new string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// The string (treated as an array) of nominal character features.
        /// </summary>
        private string features;

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>The features.</value>
        /// <remarks></remarks>
        public new string Features
        {
            get { return features; }
            set { features = value; }
        }

        /// <summary>
        /// Instance weighting that may be used in some calculations.
        /// </summary>
        private float weight;

        /// <summary>
        /// Gets or sets the instance weight.
        /// </summary>
        /// <value>The weight.</value>
        /// <remarks></remarks>
        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        /// <summary>
        /// Feature weights that may be used in some calculations.
        /// </summary>
        private float[] weights;

        /// <summary>
        /// Gets or sets the weights.
        /// </summary>
        /// <value>The weights.</value>
        /// <remarks></remarks>
        public float[] Weights
        {
            get { return weights; }
            set { weights = value; }
        }

        /// <summary>
        /// Gets the <see cref="System.Double"/> at the specified index.
        /// </summary>
        /// <remarks></remarks>
        public new double this[int index]
        {
            get
            {
                return Features[index];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceS"/> class.
        /// </summary>
        /// <param name="class_">The class_.</param>
        /// <remarks></remarks>
        public InstanceS(string class_)
        {
            this.features = class_;
            this._class = class_;
            this.weights = new float[features.Length];
            for (int i = 0; i < weights.Length; i++) 
                weights[i] = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceS"/> class.
        /// </summary>
        /// <param name="class_">The class_.</param>
        /// <param name="features">The features.</param>
        /// <remarks></remarks>
        public InstanceS(string class_, string features)
        {
            this.features = features;
            this._class = class_;
            this.weights = new float[features.Length];
            for (int i = 0; i < weights.Length; i++) 
                weights[i] = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceS"/> class.
        /// </summary>
        /// <param name="class_">The class_.</param>
        /// <param name="features">The features.</param>
        /// <param name="weights">The weights.</param>
        /// <remarks></remarks>
        public InstanceS(string class_, string features, float[] weights)
        {
            this.features = features;
            this._class = class_;
            this.weights = weights;
        }

        /// <summary>
        /// Calculates the Hamming distance of this instance to a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The Hamming distance.</returns>
        /// <remarks></remarks>
        public int HammingDistance(InstanceS vector)
        {
            int distance, len;

            if (Features.Length != vector.Features.Length) 
                return int.MaxValue;

            if (Features.Length < vector.Features.Length)
            {
                distance = vector.Features.Length - Features.Length;
                len = Features.Length;
            }
            else
            {
                distance = Features.Length - vector.Features.Length;
                len = vector.Features.Length;
            }

            for (int i = 0; i < len; i++)
            {
                distance += (Features[i] != vector.Features[i]) ? 1 : 0;
            }

            return distance;
        }

        /// <summary>
        /// Calculates the weighted Hamming distance of this instance to a specified instance.
        /// </summary>
        /// <param name="vector">The instance.</param>
        /// <returns>The weighted Hamming distance.</returns>
        /// <remarks></remarks>
        public float ConfusionWeightedHammingDistance(InstanceS vector)
        {
            if (Features.Length != vector.Features.Length) return float.MaxValue;

            // empirical confusion probabilities
            var confusions = new Dictionary<string, float>()
            {
                { "F, E", 0.314f },
                { "E, F", 0.147f },
                { "Q, O", 0.125f },
                { "O, Q", 0.125f },
                { "D, O", 0.105f },
                { "O, D", 0.105f },
                { "I, J", 0.105f },
                { "R, K", 0.118f },
                { "N, H", 0.102f },
                { "P, F", 0.094f },
                { "H, N", 0.093f },
                { "L, E", 0.090f },
                { "I, Z", 0.085f },
                { "J, I", 0.080f },
                { "X, Z", 0.074f },
                { "Y, A", 0.073f },
                { "X, I", 0.072f },
                { "A, V", 0.063f },
                { "N, O", 0.057f },
                { "V, A", 0.057f },
                { "W, M", 0.055f },
                { "A, Y", 0.054f },
                { "B, R", 0.053f },
                { "Z, I", 0.052f }
            };


            float distance = 0;
            int errCount = 0;
            const float confusionMultiplier = 2.5f;

            for (int i = 0; i < Features.Length; i++)
            {
                if (Features[i] != vector.Features[i])
                {
                    string conf = "" + vector.Features[i] + ", " + Features[i];
                    if (confusions.ContainsKey(conf))
                    {
                        distance += Math.Max(0, 1 - (confusionMultiplier * confusions[conf]));
                    }
                    else
                    {
                        distance += 1;
                    }
                    errCount += 1;
                }
            }

            distance += (1 - Weight) * errCount * 0.5f;

            return distance;
        }
    }
}
