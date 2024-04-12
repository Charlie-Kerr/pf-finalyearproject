//References: https://github.com/k13n/soliton_distribution/blob/master/src/main/java/soliton/RobustSolitonGenerator.java
//Used for the RSD class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FYP
{
    public enum SolitonDistributionType
    {
        ISD, RSD
    }
    public abstract class DegreeDistribution
    {
        protected int N; //size of data
        public DegreeDistribution(int N) {
            this.N = N;
        }

        public abstract int next();
    }

    public class RSD : DegreeDistribution 
    {
        //Random Soliton Distribution
        private double[] isdWeights;
        private double[] rsdWeights;
        private double[] normalisedRSDWeights;
        private Random random;
        private int spike; // M
        private double c;
        private double R;
        private double delta; //chance of failure
        private double beta; //normalisation factor

        public RSD(int N, double c, double delta) : base(N) 
        {
            random = new Random();
            isdWeights = new double[N];
            generateISDWeights();

            this.N = N;
            this.c = c;
            this.delta = delta;

            R = calculateR();
            spike = calculateSpike();

            rsdWeights = new double[N];
            generateRSDWeights();

            beta = calculateBeta();

            normalisedRSDWeights = new double[N];
            generateNormalisedRSD();

        }
        public override int next() 
        {
            double u = random.NextDouble();
            return inverseTransformSampling(u);
        }

        private int inverseTransformSampling(double u) //binary search through the normalised Robust Soliton Distribution weights
        {
            int low = 0;
            int high = N - 1;
            int mid = 0;

            if (u <= normalisedRSDWeights[0]) return 1;

            while (low <= high)
            {
                mid = (int)Math.Ceiling((low + high) / 2.0);
                if (mid == 0) return mid + 2;
                if (normalisedRSDWeights[mid - 1] < u && u <= normalisedRSDWeights[mid]) return mid + 1; //returns degree
                else if (u >= normalisedRSDWeights[mid]) low = mid + 1;
                else high = mid - 1;
            }
            return mid + 1;
        }

        private double calculateR()
        {
            return c * Math.Log(N / delta) * Math.Sqrt(N);
        }
        private int calculateSpike()
        {
            return (int)Math.Floor(N / R);
        }

        private void generateNormalisedRSD()
        {
            for(int i = 0; i < N; i++)
            {
                normalisedRSDWeights[i] = (isdWeights[i] + rsdWeights[i]) / beta;
            }
        }

        private double calculateBeta()
        {   
            return isdWeights[isdWeights.Length - 1] + rsdWeights[rsdWeights.Length - 1];
        }
        private void generateISDWeights() //culmulative distribution function
        {
            isdWeights[0] = 1.0 / N;

            for (int i = 1; i < N; i++)
            {
                isdWeights[i] = isdWeights[i - 1] + pdf(i + 1);
            }
        }

        private void generateRSDWeights() //culmulative distribution function
        {
            rsdWeights[0] = 0;

            for (int i = 1; i < N; i++)
            {
                if (1 <= i && i <= spike - 1)
                {
                    rsdWeights[i] = rsdWeights[i - 1] + (1.0 / (i * spike));
                }
                else if (i == spike)
                {
                    rsdWeights[i] = rsdWeights[i - 1] + Math.Log(R / delta) / spike;
                }
                else 
                {
                    rsdWeights[i] = rsdWeights[i - 1] + 0;
                }
            }
        }
        private double pdf(double x) //probability density function
        {
            return 1.0 / (x * (x - 1));
        }
    }

    public class ISD : DegreeDistribution 
    {
        //Ideal Soliton Distribution
        private double[] weights;
        private Random random;
        public ISD(int N) : base(N) 
        {
            if (N <= 0)
            {
                throw new Exception("N must be greater than 0");
            }
            this.weights = new double[N];
            this.random = new Random();
            generateWeights();
        }

        public ISD(int N, int seed) : base(N)
        {
            if (N <= 0) {
                throw new Exception("N must be greater than 0");
            }
            this.N = N;
            this.random = new Random(seed);
            this.weights = new double[N];
            generateWeights();
        }

        public int getDegreeOfWeight(int i) //position i is probability of degree i+1
        { 
            //primarly used for testing
            if (i < 0 || i >= N)
            {
                throw new Exception("Index out of bounds");
            }
            return binarySearch(weights[i]);
        }

        public override int next()
        {
            double p = random.NextDouble(); //uniform probability
            if (p <= weights[0]) return 1;
            else return binarySearch(p);
        }

        private int binarySearch(double p) //Searches using index, returns degree
        {
            int low = 0;
            int high = N - 1;

            if (p <= weights[0]) return 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (mid == 0) return mid + 2;
                if (weights[mid-1] < p && p <= weights[mid]) return mid + 1; //returns degree
                else if (p >= weights[mid]) low = mid + 1;
                else high = mid - 1;
            }
            return -1;
        }

        private void generateWeights() //culmulative distribution function
        {
            weights[0] = 1.0 / N;

            for (int i = 1; i < N; i++)
            {
                weights[i] = weights[i - 1] + pdf(i+1);
            }
        }

        private double pdf(double x) //probability density function
        {
            return 1.0 / (x * (x - 1));
        }
    }
}
