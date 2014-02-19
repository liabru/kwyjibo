using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Kwyjibo.ML.Classifiers;
using Kwyjibo.ML.Features;
using Kwyjibo.ML.Instances;
using System.IO;

namespace Kwyjibo.ML.Validation
{
    /// <summary>
    /// A system for automating classifier validation.
    /// </summary>
    /// <remarks></remarks>
    public class ValidationSuite
    {
        /// <summary>
        /// The list of jobs that will be performed.
        /// </summary>
        private List<ValidationJob> jobs;

        /// <summary>
        /// Gets or sets the jobs.
        /// </summary>
        /// <value>The jobs.</value>
        /// <remarks></remarks>
        public List<ValidationJob> Jobs
        {
            get { return jobs; }
            set { jobs = value; }
        }

        /// <summary>
        /// The list of results after performing the jobs.
        /// </summary>
        private List<ValidationJobResult> results;

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        /// <remarks></remarks>
        public List<ValidationJobResult> Results
        {
            get { return results; }
            set { results = value; }
        }

        /// <summary>
        /// Notes that will be printed out in the logs.
        /// </summary>
        private string notes;

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        /// <remarks></remarks>
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        /// <summary>
        /// Records the duration of running all jobs.
        /// </summary>
        private TimeSpan duration;

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        /// <remarks></remarks>
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public ValidationSuite()
        {
            jobs = new List<ValidationJob>();
            results = new List<ValidationJobResult>();
            notes = "No notes.";
        }

        /// <summary>
        /// Performs all jobs.
        /// </summary>
        /// <remarks></remarks>
        public void PerformAllJobs()
        {
            Results = new List<ValidationJobResult>();

            Stopwatch timer = new Stopwatch();
            timer.Start();

            int jn = 1;
            foreach (ValidationJob j in Jobs)
            {
                ValidationJobResult res = j.Perform();
                res.JobNumber = jn;
                Results.Add(res);
                jn += 1;
            }

            timer.Stop();
            Duration = timer.Elapsed;
        }

        /// <summary>
        /// Saves results to file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <remarks></remarks>
        public void SaveToFile(string path)
        {
            int i = 1;
            while (File.Exists(path + "results_" + i + ".txt"))
            {
                i += 1;
            }
            File.WriteAllText(path + "results_" + i + ".txt", Print());
            File.WriteAllText(path + "results_" + i + ".csv", PrintCSV());
        }

        /// <summary>
        /// Pretty prints the results.
        /// </summary>
        /// <returns>A pretty string of results.</returns>
        /// <remarks></remarks>
        public string Print()
        {
            string str = "";
            str += "Validation Job Results \n======================\n\n";
            str += DateTime.Now.ToString() + "\n\n";
            str += Notes + "\n\n";
            str += Jobs.Count + " jobs took " + Duration.TotalSeconds.ToString("F2") + " s\n\n";
            str += "========================================================\n";
            str += "  Acc    Rate         Var    Conf   Job   Ne  Reduc  Nf\n";
            str += "========================================================\n\n";

            return str + PrintResultSummary() + "\n\n" + PrintAllResults();
        }

        /// <summary>
        /// Prints the CSV.
        /// </summary>
        /// <returns>A CSV string of results.</returns>
        /// <remarks></remarks>
        public string PrintCSV()
        {
            return PrintResultSummary();
        }

        /// <summary>
        /// Prints all results.
        /// </summary>
        /// <returns>A pretty string of results.</returns>
        /// <remarks></remarks>
        public string PrintAllResults()
        {
            string str = "";
            foreach (ValidationJobResult res in Results) 
                str += res.ToString();
            return str;
        }

        /// <summary>
        /// Prints the result summary.
        /// </summary>
        /// <returns>A pretty result summary.</returns>
        /// <remarks></remarks>
        public string PrintResultSummary()
        {
            List<ValidationJobResult> sorted = Results.OrderByDescending(j => j.Confusion.Accuracy).ThenBy(j => j.Duration).ToList();
            string str = "";
            foreach (ValidationJobResult res in sorted)
            {
                str += res.Confusion.Accuracy.ToString("F4") + ", " + res.Confusion.Rate.ToString("F2")
                    + ", \t" + res.Confusion.Variance.ToString("F3") + ", " + res.Confusion.ConfidenceInterval.ToString("F3")
                    + ", #" + res.JobNumber + ", \t" + res.Confusion.ExampleCount + ", " + res.Job.Notes + ", " + res.Job.Classifier.ToString() + "\n";
            }
            return str;
        }
    }

    /// <summary>
    /// Represents a single validation job to be performed.
    /// </summary>
    /// <remarks></remarks>
    public class ValidationJob
    {
        /// <summary>
        /// The classifier that will be validated.
        /// </summary>
        private Classifier classifier;

        /// <summary>
        /// Gets or sets the classifier.
        /// </summary>
        /// <value>The classifier.</value>
        /// <remarks></remarks>
        public Classifier Classifier
        {
            get { return classifier; }
            set { classifier = value; }
        }

        /// <summary>
        /// The validator alorithm that will perform evaluation.
        /// </summary>
        private Validator validator;

        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>The validator.</value>
        /// <remarks></remarks>
        public Validator Validator
        {
            get { return validator; }
            set { validator = value; }
        }

        /// <summary>
        /// Notes that will be printed in the logs.
        /// </summary>
        private string notes = "None.";

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        /// <remarks></remarks>
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        /// <summary>
        /// Performs this job.
        /// </summary>
        /// <returns>The validation results.</returns>
        /// <remarks></remarks>
        public ValidationJobResult Perform()
        {
            Validator.TrainingFunction = delegate(List<Instance> train)
            {
                Classifier.Train(train);
            };

            Validator.ClassifyFunction = delegate(Instance test)
            {
                return Classifier.Classify(test);
            };

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Validator.Compute();
            timer.Stop();

            return new ValidationJobResult(this, Validator.ConfusionMatrix, timer.Elapsed);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return "Name: " + Notes + "\n\n" + Validator.ToString() + "\n\n" + Classifier.ToString();
        }
    }

    /// <summary>
    /// Represents the results of a validation job.
    /// </summary>
    /// <remarks></remarks>
    public struct ValidationJobResult 
    {
        /// <summary>
        /// The job that generated these results.
        /// </summary>
        public ValidationJob Job;

        /// <summary>
        /// The amount of time taken to perfom this job.
        /// </summary>
        public TimeSpan Duration;

        /// <summary>
        /// The confusion matrix generated by this job.
        /// </summary>
        public ClassConfusionMatrix Confusion;

        /// <summary>
        /// The number of this job.
        /// </summary>
        public int JobNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationJobResult"/> struct.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="confusion">The confusion.</param>
        /// <param name="duration">The duration.</param>
        /// <remarks></remarks>
        public ValidationJobResult(ValidationJob job, ClassConfusionMatrix confusion, TimeSpan duration)
        {
            Job = job;
            Duration = duration;
            Confusion = confusion;
            JobNumber = 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return "Validation Job #" + JobNumber + "\n======================\n\n" + Job.ToString() + "\n\nTook " + Duration.TotalSeconds.ToString("F3") + "s."
                + "\n\n" + Confusion.ToString() + "\n";
        }
    }
}
