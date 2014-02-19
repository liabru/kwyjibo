using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Kwyjibo.ML.Classifiers;
using Kwyjibo.ML.Features;
using Kwyjibo.ML.Instances;
using Kwyjibo.ML.Validation;
using AForge.Math;
using System.IO;

namespace Kwyjibo.UserInterface.Forms
{
    public partial class ValidationForm : Form
    {
        public ValidationForm()
        {
            InitializeComponent();

            string inpath = @"D:\Liam\Dropbox\Uni\3rd Year\Dissertation\Data\NewTraining";
            //string inpath = @"D:\Liam\Dropbox\Uni\3rd Year\Dissertation\Data\BlurTraining";
            string outpath = @"D:\Liam\Dropbox\Uni\3rd Year\Dissertation\Logs\ValidationNew\tests\";

            /*
            for (int i = 8; i <= 8; i++)
            {
                RunTests("grid", @"C:\Users\Liam\Desktop\training_split\" + i, outpath + i + "_grid_");
                RunTests("merge", @"C:\Users\Liam\Desktop\training_split\" + i, outpath + i + "_merge_");
            }
            */
            
            RunTests("grid", inpath, outpath + "grid_");
            RunTests("merge", inpath, outpath + "merge_");
            
        }

        public void Print(string text)
        {
            logText.Text += text;
        }

        public void GenerateJobs(ValidationSuite vs, List<Instance> instances, string notes)
        {
            ValidationJob job;

            int folds = 10, reps = 2;

            job = new ValidationJob();
            job.Validator = new KFoldValidation(instances, folds, reps);
            job.Classifier = new NaiveBayesClassifier();
            job.Notes = notes;

            vs.Jobs.Add(job);
            /*
            for (int k = 1; k <= 4; k++)
            {
                foreach (Metric metric in Enum.GetValues(typeof(Metric)))
                {
                    if (metric == Metric.HammingDistance || metric == Metric.ChebyshevDistance 
                        || metric == Metric.JaccardSimilarity || metric == Metric.DistanceSquared) continue;

                    WeightMode mode = WeightMode.InverseDistance;
                    job = new ValidationJob();
                    job.Validator = new KFoldValidation(instances, folds, reps);
                    job.Classifier = new KNearestClassifier(k, metric, mode);
                    job.Notes = notes;

                    vs.Jobs.Add(job);
                }
            }*/
        }

        public void RunTests(string method, string inPath, string outPath)
        {
            List<Instance> training;
            ValidationSuite vs = new ValidationSuite();
            string path = inPath;
            string fr = method + ",";

            vs.Notes = "Instances loaded from: " + path;
            vs.Notes += "\nFeature Reduction: " + fr;
            
            int[] ns = new int[] {16, 32, 64, 128, 256, 512, 768, 1024};
            //int[] ns = new int[] { 16, 32, 64, 128, 256, 512 };
            foreach(int n in ns)
            {
                training = LoadInstancesFromBitmaps(path, n, method);
                GenerateJobs(vs, training, fr+" "+n);
            }

            vs.PerformAllJobs();
            vs.SaveToFile(outPath);
            Print(vs.Print());
        }

        public List<Bitmap> LoadAllBitmaps(string path)
        {
            List<Bitmap> bmps = new List<Bitmap>();
            string[] files = Directory.GetFiles(path, "*.bmp", SearchOption.AllDirectories);
            foreach (string fp in files)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(fp);
                bmp.Tag = Path.GetFileNameWithoutExtension(fp);
            }
            return bmps;
        }

        public List<Instance> LoadInstancesFromBitmaps(string path, int n, string method)
        {
            List<Instance> instances = new List<Instance>();
            string[] files = Directory.GetFiles(path, "*.bmp", SearchOption.AllDirectories);
            foreach (string fp in files)
            {
                string name = Path.GetFileNameWithoutExtension(fp);
                name = name.Replace("letter_", "");
                Bitmap bmp = (Bitmap)Bitmap.FromFile(fp);
                double[] features = process(bmp, n, method);
                instances.Add(new Instance(name, features));
            }
            return instances;
        }

        private double[] process(Bitmap bmp, int n, string method)
        {
            double[,] features2D = BitmapFeatures.Intensity(bmp);

            switch (method)
            {
                case "merge":
                    double[] features = FeatureReduction.Flatten(features2D);
                    return FeatureReduction.Merge(features, n);
                case "grid":
                    return FeatureReduction.GridwiseMerge(features2D, n);
            }

            return FeatureReduction.Flatten(features2D);

            //double[] features = FeatureReduction.Flatten(features2D);
            //features = FeatureReduction.RowColSums(features2D);
            //features = FeatureReduction.LDA(features2D, 4);
            //features = FeatureReduction.PCA(features2D, 4);
            //features = FeatureReduction.SVD(features2D);
            //features = FeatureReduction.FHT(features2D);
            //features = FeatureReduction.Sort(features);
            //features = FeatureReduction.Covariance(features2D);
            //features = FeatureReduction.Merge(features, n);
            //features = FeatureReduction.Binarize(features);
            //double[] features = FeatureReduction.GridwiseMerge(features2D, n);
            //features = FeatureReduction.Discretize(features, 100);
            //features = FeatureReduction.Binarize(features);
            //return features;
        }
        
        public double[] MakeDouble(Complex[,] comp)
        {
            int width = comp.GetUpperBound(0) + 1;
            int height = comp.GetUpperBound(1) + 1;

            double[] features = new double[width * height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    features[(j * width) + i] = comp[i, j].Im;
                }
            }

            return features;
        }

        public double[] MakeDouble(Complex[] comp)
        {
            double[] features = new double[comp.Length];

            for (int i = 0; i < features.Length; i++)
            {
                features[i] = comp[i].Re;
            }

            return features;
        }

        public Complex[] MakeComplex(double[] features)
        {
            Complex[] comp = new Complex[features.Length];

            for (int i = 0; i < features.Length; i++)
            {
                comp[i] = new Complex(features[i], 0);
            }

            return comp;
        }

        public Complex[,] MakeComplex(double[,] features)
        {
            int width = features.GetUpperBound(0) + 1;
            int height = features.GetUpperBound(1) + 1;

            Complex[,] comp = new Complex[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    comp[i, j] = new Complex(features[i, j], 0);
                }
            }

            return comp;
        }
    }
}
