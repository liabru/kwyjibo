using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kwyjibo.ML.Instances;
using Kwyjibo.ML.Features;

namespace Kwyjibo.ML.Classifiers
{
    public enum Metric { DistanceSquared, EuclideanDistance, ManhattanDistance, ChebyshevDistance, CosineSimilarity, PearsonSimilarity, JaccardSimilarity, HammingDistance, WeightedHammingDistance }

    public enum WeightMode { Modal, InverseDistance }

    /// <summary>
    /// Implementation of the k-Nearest-Neighbour classification algorithm.
    /// </summary>
    /// <remarks></remarks>
    public class KNearestClassifier : Classifier
    {
        /// <summary>
        /// The number of instances to consider when deciding the classification.
        /// </summary>
        private int k;

        /// <summary>
        /// Gets or sets the K value.
        /// </summary>
        /// <value>The K value.</value>
        /// <remarks></remarks>
        public int K
        {
            get { return k; }
            set { k = value; }
        }

        /// <summary>
        /// The distance or similarity metric used during classification.
        /// </summary>
        private Metric metric;

        /// <summary>
        /// Gets or sets the metric.
        /// </summary>
        /// <value>The metric.</value>
        /// <remarks></remarks>
        public Metric Metric
        {
            get { return metric; }
            set { metric = value; }
        }

        /// <summary>
        /// The method of weighting the k-Nearest to produce a result.
        /// </summary>
        private WeightMode mode;

        /// <summary>
        /// Gets or sets the weight mode.
        /// </summary>
        /// <value>The weight mode.</value>
        /// <remarks></remarks>
        public WeightMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        /// The set of training instances used to classify.
        /// </summary>
        private List<Instance> trainingInstances;

        /// <summary>
        /// Gets or sets the training instances.
        /// </summary>
        /// <value>The training instances.</value>
        /// <remarks></remarks>
        public List<Instance> TrainingInstances
        {
            get { return trainingInstances; }
            set { trainingInstances = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public KNearestClassifier()
        {
            k = 1;
            metric = Metric.DistanceSquared;
            mode = WeightMode.Modal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KNearestClassifier"/> class.
        /// </summary>
        /// <param name="k">The k value.</param>
        /// <param name="metric">The metric.</param>
        /// <param name="mode">The weight mode.</param>
        /// <remarks></remarks>
        public KNearestClassifier(int k, Metric metric, WeightMode mode)
        {
            this.k = k;
            this.metric = metric;
            this.mode = mode;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.GetType().Name + ", " + K + ", " 
                    + Enum.GetName(typeof(Metric), Metric) 
                    + ", " + Enum.GetName(typeof(WeightMode), Mode);
        }

        /// <summary>
        /// Trains the classifier on the specified training instances.
        /// </summary>
        /// <param name="trainingInstances">The training instances.</param>
        /// <remarks></remarks>
        public override void Train(List<Instance> trainingInstances)
        {
            this.TrainingInstances = trainingInstances;
        }

        /// <summary>
        /// Classifies the specified test instance.
        /// </summary>
        /// <param name="test">The test instance.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override Result Classify(Instance test)
        {
            return FindMaxClass(FindKNearest(test));
        }

        /// <summary>
        /// Finds the K nearest instances to the test instance.
        /// </summary>
        /// <param name="test">The test instance.</param>
        /// <returns>An array of the k-nearest results.</returns>
        /// <remarks></remarks>
        public Result[] FindKNearest(Instance test)
        {
            List<Result> results = new List<Result>();

            foreach (Instance train in TrainingInstances)
            {
                double dist = double.MaxValue;

                switch (Metric)
                {
                    case Metric.DistanceSquared:   dist = train.DistanceSquared(test);        break;
                    case Metric.EuclideanDistance: dist = train.EuclideanDistance(test);      break;
                    case Metric.ManhattanDistance: dist = train.ManhattanDistance(test);      break;
                    case Metric.ChebyshevDistance: dist = train.ChebyshevDistance(test);      break;
                    case Metric.CosineSimilarity:  dist = 1 - train.CosineSimilarity(test);   break;
                    case Metric.PearsonSimilarity: dist = 1 - train.PearsonSimilarity(test);  break;
                    case Metric.JaccardSimilarity: dist = 1 - train.JaccardSimilarity(test);  break;
                    default: throw new NotSupportedException();
                }

                results.Add(new Result(train.Class, dist));
            }

            int k = (int)Math.Min(K, TrainingInstances.Count);
            results.Sort();
            Result[] knr = new Result[k];
            results.CopyTo(0, knr, 0, k);

            return knr;
        }

        /// <summary>
        /// Finds the K nearest instances to a test instance. String based version.
        /// </summary>
        /// <param name="test">The test instance.</param>
        /// <returns>An array of the k-nearest results.</returns>
        /// <remarks></remarks>
        public Result[] FindKNearest(InstanceS test)
        {
            List<Result> results = new List<Result>();

            foreach (InstanceS train in TrainingInstances)
            {
                double dist = double.MaxValue;

                switch (Metric)
                {
                    case Metric.HammingDistance: dist = train.HammingDistance(test); break;
                    case Metric.WeightedHammingDistance: dist = train.ConfusionWeightedHammingDistance(test); break;
                    default: throw new NotSupportedException();
                }

                results.Add(new Result(train.Class, dist));
            }

            int k = (int)Math.Min(K, TrainingInstances.Count);
            results.Sort();
            Result[] knr = new Result[k];
            results.CopyTo(0, knr, 0, k);

            return knr;
        }

        /// <summary>
        /// Finds the max class.
        /// </summary>
        /// <param name="results">The k-nearest results array to process.</param>
        /// <returns>The calculated classification based on the given results.</returns>
        /// <remarks></remarks>
        private Result FindMaxClass(Result[] results)
        {
            Dictionary<string, double> scores = new Dictionary<string, double>();

            for (int i = 0; i < results.Length; i++)
            {
                scores[results[i].Class] = 0;
            }

            for (int i = 0; i < results.Length; i++)
            {
                switch (Mode)
                {
                    case WeightMode.Modal:           scores[results[i].Class] += 1;                          break;
                    case WeightMode.InverseDistance: scores[results[i].Class] += 1 / (1 + results[i].Value); break;
                }
            }

            string bestClass = "?";
            double bestValue = 0;

            foreach (var s in scores)
            {
                if (s.Value > bestValue)
                {
                    bestClass = s.Key;
                    bestValue = s.Value;
                }
            }

            return new Result(bestClass, bestValue);
        }
    }
}
