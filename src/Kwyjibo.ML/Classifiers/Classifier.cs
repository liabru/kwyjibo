using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kwyjibo.ML.Instances;

namespace Kwyjibo.ML.Classifiers
{
    /// <summary>
    /// Abstract classifier type.
    /// </summary>
    /// <remarks></remarks>
    public abstract class Classifier
    {
        /// <summary>
        /// Trains the specified training instances.
        /// </summary>
        /// <param name="trainingInstances">The training instances.</param>
        /// <remarks></remarks>
        public abstract void Train(List<Instance> trainingInstances);

        /// <summary>
        /// Classifies the specified test instance.
        /// </summary>
        /// <param name="test">The test instance.</param>
        /// <returns>A classification result.</returns>
        /// <remarks></remarks>
        public abstract Result Classify(Instance test);
    }
}
