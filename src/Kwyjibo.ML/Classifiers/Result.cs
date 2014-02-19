using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kwyjibo.ML.Instances;

namespace Kwyjibo.ML.Classifiers
{
    /// <summary>
    /// Stores the details of a classification result.
    /// </summary>
    /// <remarks></remarks>
    public struct Result : IComparable<Result>
    {
        /// <summary>
        /// The predicted class.
        /// </summary>
        private string _class;

        /// <summary>
        /// Gets or sets the predicted class.
        /// </summary>
        /// <value>The class.</value>
        /// <remarks></remarks>
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// A number given by the classifier usually representing the maximum similarity, distance or probability found that was used for this classification.
        /// </summary>
        private double value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <remarks></remarks>
        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="_class">The _class.</param>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        public Result(string _class, double value)
        {
            this.value = value;
            this._class = _class;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return "(" + Class + ", " + Value + ")";
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.</returns>
        /// <remarks></remarks>
        public int CompareTo(Result other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}
