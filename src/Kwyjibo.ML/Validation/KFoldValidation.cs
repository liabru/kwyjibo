using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kwyjibo.ML.Instances;
using Kwyjibo.ML.Classifiers;
using System.Diagnostics;

namespace Kwyjibo.ML.Validation
{
    /// <summary>
    /// Implementation of k-fold cross validation. Generates a confusion matrix of results.
    /// </summary>
    /// <remarks></remarks>
    public class KFoldValidation : Validator
    {
        /// <summary>
        /// The whole set of instances to validate.
        /// </summary>
        private List<Instance> instances;

        /// <summary>
        /// Gets or sets the instances.
        /// </summary>
        /// <value>The instances.</value>
        /// <remarks></remarks>
        public List<Instance> Instances
        {
            get { return instances; }
            set { instances = value; }
        }

        /// <summary>
        /// The number of folds to use, a good value is 10.
        /// </summary>
        private int folds;

        /// <summary>
        /// Gets or sets the folds.
        /// </summary>
        /// <value>The the number of folds.</value>
        /// <remarks></remarks>
        public int Folds
        {
            get { return folds; }
            set { folds = value; }
        }

        /// <summary>
        /// The number of times to repeat the validation process, to provide an average.
        /// </summary>
        private int repetitions;

        /// <summary>
        /// Gets or sets the number of repetitions.
        /// </summary>
        /// <value>The number of repetitions.</value>
        /// <remarks></remarks>
        public int Repetitions
        {
            get { return repetitions; }
            set { repetitions = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KFoldValidation"/> class.
        /// </summary>
        /// <param name="instances">The instances.</param>
        /// <param name="folds">The the number of folds.</param>
        /// <param name="repetitions">The the number of repetitions.</param>
        /// <remarks></remarks>
        public KFoldValidation(List<Instance> instances, int folds, int repetitions)
        {
            this.instances = instances;
            this.folds = folds;
            this.repetitions = repetitions;
            this.ConfusionMatrix = new ClassConfusionMatrix();
        }

        /// <summary>
        /// Runs the validation process. Results are avaliable in the form of the ConfusionMatrix property.
        /// </summary>
        /// <remarks></remarks>
        public override void Compute()
        {
            ConfusionMatrix = new ClassConfusionMatrix();
            ConfusionMatrix.SampleSize = (int)Math.Ceiling((double)Instances.Count / Folds);
            ConfusionMatrix.ExampleCount = Instances.Count;

            Stopwatch timer = new Stopwatch();

            for (int i = 0; i < Repetitions; i += 1)
            {
                List<Instance[]> instFolds = CreateFolds(Instances, Folds);

                for (int j = 0; j < instFolds.Count; j++)
                {
                    Instance[] curFold = instFolds[j];

                    List<Instance> train = new List<Instance>();
                    for (int k = 0; k < instFolds.Count; k++) 
                    {
                        if (k != j)
                        {
                            train.AddRange(instFolds[k]);
                        }
                    }

                    TrainingFunction(train);

                    for (int k = 0; k < curFold.Length; k++) 
                    {
                        Instance x = curFold[k];
                        timer.Start();
                        Result p = ClassifyFunction(x);
                        timer.Stop();
                        ConfusionMatrix[x.Class, p.Class] += 1;
                    }
                }
            }

            ConfusionMatrix.Rate = (float)ConfusionMatrix.Observations / (float)timer.Elapsed.TotalSeconds;
        }

        /// <summary>
        /// Creates the folds.
        /// </summary>
        /// <param name="instances">The instances.</param>
        /// <param name="folds">The number of folds.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<Instance[]> CreateFolds(List<Instance> instances, int folds)
        {
            List<Instance> copy = new List<Instance>(instances);
            List<Instance[]> instFolds = new List<Instance[]>();
            Random rand = new Random();
            int foldSize = (int)Math.Ceiling((double)instances.Count / folds);

            for (int i = 0; i < folds; i++)
            {
                int size = Math.Min(foldSize, copy.Count);
                instFolds.Add(new Instance[size]);

                for (int j = 0; j < size; j++)
                {
                    int r = rand.Next(copy.Count);
                    instFolds[i][j] = copy[r];
                    copy.RemoveAt(r);
                }
            }

            return instFolds;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.GetType().Name + ", " + "Folds = " + Folds + ", Repetitions = "
                    + Repetitions + ", Examples = " + Instances.Count + ".";
        }
    }
}
