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
        protected int degree;
        protected int N; //size of data
        public DegreeDistribution(int k, int N) {
            this.degree = k;
            this.N = N;
        }
    }

    internal class ISD : DegreeDistribution 
    {
        public double[] weights;
        //Ideal Soliton Distribution
        public ISD(int k, int N) : base(k, N) 
        { 
            this.degree = k;
            this.weights = new double[N];
        }

        public void generateWeights()
        {
            weights[0] = 1.0 / N;

            for (int i = 2; i < N; i++)
            {
                weights[i - 1] = weights[i - 2] + pdf(i);
                Console.WriteLine(weights[i - 1]);
            }
        }

        private double pdf(double x) 
        {
            return 1.0 / (x * (x - 1));
        }

    }
}
