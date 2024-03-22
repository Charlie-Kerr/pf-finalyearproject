using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FYP
{
    internal class DegreeDistribution
    {
        protected int N; //size of data
        public DegreeDistribution(int N) {
            this.N = N;
        }
    }

    internal class ISD : DegreeDistribution 
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
