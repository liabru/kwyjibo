using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kwyjibo.ML.Instances;
using Kwyjibo.ML.Features;

namespace Kwyjibo.ML.Classifiers
{
    /// <summary>
    /// An implementation of the naieve Bayesian classification algorithm.
    /// </summary>
    /// <remarks></remarks>
    public class NaiveBayesClassifier : Classifier
    {
        /// <summary>
        /// The classifier distribution model, learned during training. It is a map of observations to calculated probabilities.
        /// </summary>
        private Dictionary<string, double> Model;

        /// <summary>
        /// A record of all classes encountered during training, and the counts of each observed.
        /// </summary>
        private Dictionary<string, double> Classes;

        /// <summary>
        /// The total number of examples seen during training.
        /// </summary>
        private int TotalExamples;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Trains the classifier on the specified training instances.
        /// </summary>
        /// <param name="trainingInstances">The training instances.</param>
        /// <remarks></remarks>
        public override void Train(List<Instance> trainingInstances)
        {
            Model = new Dictionary<string, double>();
            Classes = new Dictionary<string, double>();
            TotalExamples = trainingInstances.Count;

            foreach (Instance train in trainingInstances)
            {
                double[] features = FeatureReduction.Discretize(train.Features, 3);

                for (int i = 0; i < features.Length; i++)
                {
                    string c = train.Class + "|" + i + "|" + features[i];

                    if (Model.ContainsKey(c))
                    {
                        Model[c] += 1;
                    } 
                    else 
                    {
                        Model[c] = 1;
                    }
                }

                if (Classes.ContainsKey(train.Class))
                {
                    Classes[train.Class] += 1;
                }
                else
                {
                    Classes[train.Class] = 1;
                }
            }
        }

        /// <summary>
        /// Returns the recorded counts the specified class, given a certain value of a certain feature.
        /// </summary>
        /// <param name="_class">The class.</param>
        /// <param name="featureIndex">Index of the feature.</param>
        /// <param name="value">The value.</param>
        /// <returns>The count if seen, a fudge value if not seen.</returns>
        /// <remarks></remarks>
        private double Count(string _class, int featureIndex, double value)
        {
            string key = _class + "|" + featureIndex + "|" + value;
            double count;

            if (Model.TryGetValue(key, out count)) 
                return count;

            return 0.1;
        }

        /// <summary>
        /// Classifies the specified test instance.
        /// </summary>
        /// <param name="test">The test instance.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override Result Classify(Instance test)
        {
            string maxClass = "?";
            double maxProb = float.MinValue;

            double[] features = FeatureReduction.Discretize(test.Features, 10);

            foreach (KeyValuePair<string, double> pair in Classes)
            {
                double score = Math.Log(pair.Value / TotalExamples);

                for (int i = 0; i < features.Length; i++)
                {
                    score += Math.Log(Count(pair.Key, i, features[i]) / pair.Value);
                }

                if (score > maxProb)
                {
                    maxProb = score;
                    maxClass = pair.Key;
                }
            }

            return new Result(maxClass, maxProb);
        }
    }
}
