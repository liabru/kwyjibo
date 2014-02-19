using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kwyjibo.ML.Instances;
using Kwyjibo.ML.Classifiers;

namespace Kwyjibo.ML.Validation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class HoldoutValidation : Validator
    {
        /// <summary>
        /// 
        /// </summary>
        private List<Instance> training;

        /// <summary>
        /// Gets or sets the training.
        /// </summary>
        /// <value>The training.</value>
        /// <remarks></remarks>
        public List<Instance> Training
        {
            get { return training; }
            set { training = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<Instance> testing;

        /// <summary>
        /// Gets or sets the testing.
        /// </summary>
        /// <value>The testing.</value>
        /// <remarks></remarks>
        public List<Instance> Testing
        {
            get { return testing; }
            set { testing = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int folds;

        /// <summary>
        /// Gets or sets the folds.
        /// </summary>
        /// <value>The folds.</value>
        /// <remarks></remarks>
        public int Folds
        {
            get { return folds; }
            set { folds = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int repetitions;

        /// <summary>
        /// Gets or sets the repetitions.
        /// </summary>
        /// <value>The repetitions.</value>
        /// <remarks></remarks>
        public int Repetitions
        {
            get { return repetitions; }
            set { repetitions = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldoutValidation"/> class.
        /// </summary>
        /// <param name="testing">The testing.</param>
        /// <param name="training">The training.</param>
        /// <remarks></remarks>
        public HoldoutValidation(List<Instance> testing, List<Instance> training)
        {
            this.training = training;
            this.testing = testing;
            this.ConfusionMatrix = new ClassConfusionMatrix();
        }

        /// <summary>
        /// Computes this instance.
        /// </summary>
        /// <remarks></remarks>
        public override void Compute()
        {
            ConfusionMatrix = new ClassConfusionMatrix();

            TrainingFunction(Training);

            foreach (Instance test in Testing)
            {
                Result p = ClassifyFunction(test);
                ConfusionMatrix[test.Class, p.Class] += 1;
            }
        }
    }
}
