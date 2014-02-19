using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kwyjibo.ML.Classifiers;
using Kwyjibo.ML.Instances;

namespace Kwyjibo.ML.Validation
{
    /// <summary>
    /// Delegate for a classification function.
    /// </summary>
    /// <param name="test">The instance to test.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public delegate Result ValidationClassify(Instance test);

    /// <summary>
    /// Delegate for a training function.
    /// </summary>
    /// <param name="training">The training set.</param>
    /// <remarks></remarks>
    public delegate void ValidationTraining(List<Instance> training);

    /// <summary>
    /// Abstract validator class.
    /// </summary>
    /// <remarks></remarks>
    public abstract class Validator
    {
        /// <summary>
        /// Delegate for a classification function for the classifier being validated.
        /// </summary>
        private ValidationClassify classifyFunction;

        /// <summary>
        /// Gets or sets the fitting function.
        /// </summary>
        /// <value>The fitting function.</value>
        /// <remarks></remarks>
        public ValidationClassify ClassifyFunction
        {
            get { return classifyFunction; }
            set { classifyFunction = value; }
        }

        /// <summary>
        /// Delegate for the training function for the classifier being validated.
        /// </summary>
        private ValidationTraining trainingFunction;

        /// <summary>
        /// Gets or sets the training function.
        /// </summary>
        /// <value>The training function.</value>
        /// <remarks></remarks>
        public ValidationTraining TrainingFunction
        {
            get { return trainingFunction; }
            set { trainingFunction = value; }
        }

        /// <summary>
        /// The confusion matrix where the results of all tests are added.
        /// </summary>
        private ClassConfusionMatrix confusionMatrix;

        /// <summary>
        /// Gets or sets the confusion matrix.
        /// </summary>
        /// <value>The confusion matrix.</value>
        /// <remarks></remarks>
        public ClassConfusionMatrix ConfusionMatrix
        {
            get { return confusionMatrix; }
            set { confusionMatrix = value; }
        }

        /// <summary>
        /// Runs the validation process, using the specified training and classification functions.
        /// </summary>
        /// <remarks></remarks>
        public abstract void Compute();
    }
}
