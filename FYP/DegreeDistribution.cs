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
        protected int N; //size of data
        public DegreeDistribution(int N) {
            this.N = N;
        }
    }

    public class ISD : DegreeDistribution 
    {
        //Ideal Soliton Distribution
        public double[] weights;
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
        }

        public int next() 
        {
            double p = random.NextDouble(); //uniform probability
            if (p <= weights[0]) return 1;
            else return binarySearch(p);

        }

        private int binarySearch(double p)
        {
            int low = 2;
            int high = N;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                
                if (weights[mid] < p && p <= weights[mid + 1]) return mid;
                else if (p <= weights[mid]) low = mid + 1;
                else high = mid - 1;
            }
            return -1;
        }

        public void generateWeights() //culmulative distribution function
        {
            weights[0] = 1.0 / N;

            for (int i = 2; i < N; i++)
            {
                weights[i - 1] = weights[i - 2] + pdf(i);
                //Console.WriteLine(weights[i - 1]);
            }
        }

        private double pdf(double x) //probability density function
        {
            return 1.0 / (x * (x - 1));
        }

        

    }
}
