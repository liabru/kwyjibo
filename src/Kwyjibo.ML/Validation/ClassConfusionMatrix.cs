using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kwyjibo.ML.Validation
{
    /// <summary>
    /// This class represents a dynamic, arbitrary sized multi-class confusion matrix.
    /// </summary>
    /// <remarks></remarks>
    public class ClassConfusionMatrix
    {
        /// <summary>
        /// Stores the map of confusions.
        /// </summary>
        private SortedDictionary<string, SortedDictionary<string, int>> values;

        /// <summary>
        /// Stores the number of examples used by a classifier to generate this confusion matrix.
        /// </summary>
        private int exampleCount;

        /// <summary>
        /// Gets or sets the example count.
        /// </summary>
        /// <value>The example count.</value>
        /// <remarks></remarks>
        public int ExampleCount
        {
            get { return exampleCount; }
            set { exampleCount = value; }
        }

        /// <summary>
        /// Stores the number of samples used by a cross validator when generating this confusion matrix.
        /// </summary>
        private int sampleSize;

        /// <summary>
        /// Gets or sets the size of the sample.
        /// </summary>
        /// <value>The size of the sample.</value>
        /// <remarks></remarks>
        public int SampleSize
        {
            get { return sampleSize; }
            set { sampleSize = value; }
        }

        /// <summary>
        /// Stores the average rate of classification timed by a cross validator when generating this confusion matrix.
        /// </summary>
        private float rate;

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        /// <value>The rate.</value>
        /// <remarks></remarks>
        public float Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public ClassConfusionMatrix()
        {
            values = new SortedDictionary<string, SortedDictionary<string, int>>();
        }

        /// <summary>
        /// Initialises a pair of confusions in the confusion table.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <remarks></remarks>
        private void InitPair(string a, string b)
        {
            if (!values.ContainsKey(a))
            {
                values[a] = new SortedDictionary<string, int>();

                foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
                {
                    if (!pair.Value.ContainsKey(a)) 
                        pair.Value[a] = 0;
                    values[a][pair.Key] = 0;
                }
            }

            if (!values.ContainsKey(b))
            {
                values[b] = new SortedDictionary<string, int>();

                foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
                {
                    if (!pair.Value.ContainsKey(b)) 
                        pair.Value[b] = 0;
                    values[b][pair.Key] = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Int32"/> with the specified a.
        /// </summary>
        /// <remarks></remarks>
        public int this[string a, string b]
        {
            set
            {
                InitPair(a, b);
                values[a][b] = value;
            }

            get
            {
                InitPair(a, b);
                return values[a][b];
            }
        }

        /// <summary>
        /// Gets the number of observations.
        /// </summary>
        /// <remarks></remarks>
        public int Observations
        {
            get
            {
                int obs = 0;

                foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
                {
                    foreach (KeyValuePair<string, int> pair2 in pair.Value)
                    {
                        obs += pair2.Value;
                    }
                }

                return obs;
            }
        }

        /// <summary>
        /// Gets the number of correct observations.
        /// </summary>
        /// <remarks></remarks>
        public int Correct
        {
            get
            {
                int correct = 0;

                foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
                {
                    foreach (KeyValuePair<string, int> pair2 in pair.Value)
                    {
                        if (pair.Key == pair2.Key) 
                            correct += pair2.Value;
                    }
                }

                return correct;
            }
        }

        /// <summary>
        /// Gets the number of incorrect observations.
        /// </summary>
        /// <remarks></remarks>
        public int Incorrect
        {
            get
            {
                int incorrect = 0;

                foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
                {
                    foreach (KeyValuePair<string, int> pair2 in pair.Value)
                    {
                        if (pair.Key != pair2.Key) 
                            incorrect += pair2.Value;
                    }
                }

                return incorrect;
            }
        }

        /// <summary>
        /// Gets the accuracy.
        /// </summary>
        /// <remarks></remarks>
        public float Accuracy
        {
            get
            {
                return (float)Correct / Observations;
            }
        }

        /// <summary>
        /// Gets the intra-class accuracy variance.
        /// </summary>
        /// <remarks></remarks>
        public float Variance
        {
            get
            {
                float sum = 0, mean = Accuracy, maxAcc = (float)Observations / values.Count;

                foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
                {
                    float acc = 0;
                    foreach (KeyValuePair<string, int> pair2 in pair.Value)
                    {
                        if (pair.Key == pair2.Key) 
                            acc += pair2.Value;
                    }
                    acc /= maxAcc;
                    sum += (acc - mean) * (acc - mean);
                }

                return sum / values.Count;
            }
        }

        /// <summary>
        /// Gets the error rate.
        /// </summary>
        /// <remarks></remarks>
        public float Error
        {
            get
            {
                return (float)Incorrect / Observations;
            }
        }

        /// <summary>
        /// Gets the 95% confidence interval.
        /// </summary>
        /// <remarks></remarks>
        public float ConfidenceInterval
        {
            get
            {
                float err = Error;
                return (float)(1.96 * Math.Sqrt((err * (1 - err)) / SampleSize));
            }
        }

        /// <summary>
        /// Gives a pretty string containing the top N confusions.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Confusions(int n)
        {
            List<KeyValuePair<string, int>> pairs = new List<KeyValuePair<string, int>>();

            foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
            {
                foreach (KeyValuePair<string, int> pair2 in pair.Value)
                {
                    if (pair.Key != pair2.Key)
                    {
                        pairs.Add(new KeyValuePair<string, int>(pair.Key + ", " + pair2.Key, pair2.Value));
                    }
                }
            }

            pairs.Sort(delegate(KeyValuePair<String, Int32> x, KeyValuePair<String, Int32> y) {
                return -x.Value.CompareTo(y.Value); 
            });

            if (n < 0) n = pairs.Count;

            string output = "";

            int j = 0;
            for (int i = 0; i < n; i++)
            {
                if (pairs[i].Value > 0)
                {
                    output += "(" + pairs[i].Key + ", " + pairs[i].Value.ToString().PadLeft(2) + ")  ";
                    if (j == 7) output += "\n";
                    j = (j + 1) % 8;
                }
            }

            output += "\n";

            if (n != pairs.Count) 
                output += "+ " + (pairs.Count - n) + " more...\n";

            return output;
        }

        /// <summary>
        /// Returns a detailed <see cref="System.String"/> that represents this confusion matrix.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this confusion matrix.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            // Write the summary values

            string summary = "";
            summary += "Correct: " + Correct;
            summary += "  Incorrect: " + Incorrect;
            summary += "  Total: " + Observations;
            summary += "  Accuracy: " + Accuracy.ToString("F4");
            summary += "  Error: " + Error.ToString("F4");
            summary += "  Confidence: +/- " + ConfidenceInterval.ToString("F4");
            summary += "  Variance: " + Variance.ToString("F4");
            summary += "\n\n\n";

            // Calculate padding values

            int[] padding = new int[values.Count+1];
            int i;
            int sp = 2;

            foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
            {
                if (pair.Key.Length > padding[0]) 
                    padding[0] = pair.Key.Length;

                i = 1;
                foreach (KeyValuePair<string, int> pair2 in pair.Value)
                {
                    if (pair2.Value.ToString().Length > padding[i]) 
                        padding[i] = pair2.Value.ToString().Length;
                    i += 1;
                }
            }

            // Write the header
            string output = "";
            output += "".PadRight(padding[0] + 3);

            i = 1;
            foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
            {
                output += pair.Key.PadRight(padding[i] + sp);
                i += 1;
            }
            output += "\n   ".PadRight(output.Length, '-') + "\n";

            // Write each horizontal line

            foreach (KeyValuePair<string, SortedDictionary<string, int>> pair in values)
            {
                output += pair.Key.PadRight(padding[0]) + " | ";

                i = 1;
                foreach (KeyValuePair<string, int> pair2 in pair.Value)
                {
                    output += pair2.Value.ToString().PadRight(padding[i] + sp);
                    i += 1;
                }
                output += "\n";
            }

            output += "\n\nConfusions:\n\n" + Confusions(-1);

            return summary + output + "\n";
        }
    }
}
