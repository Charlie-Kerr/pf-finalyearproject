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
        //float[] probabilities;
        protected int degree;
        public DegreeDistribution(int k) {
            this.degree = k;
        }
    }

    internal class ISD : DegreeDistribution 
    { 
        public ISD(int k) : base(k) 
        { 
            this.degree = k; 
        }    

    }
}
