using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FYP
{
    public class DegreeDistribution
    {
        protected int N; //size of data (consider changing to const?)
        public DegreeDistribution(int N) {
            this.N = N;
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

        public int next() //can return zero rn!
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
                //Console.WriteLine(weights[i - 1]);
            }
        }

        private double pdf(double x) //probability density function
        {
            return 1.0 / (x * (x - 1));
        }

        

    }
}
