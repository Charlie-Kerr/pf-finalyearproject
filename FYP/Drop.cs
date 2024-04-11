using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP
{
    public class Drop
    {
        public int[] parts;
        public byte[] data;
        private int degree { get; set; }

        public Drop(int[] parts, byte[] data)
        {
            this.parts = parts;
            this.data = data;
            this.degree = parts.Length;
        }

        public int getDegree()
        {
            return degree;
        }

        public void setDegree(int degree)
        {
            this.degree = degree;
        }

        public override string ToString()
        {
            return String.Format("Drop degree is {0}\nThe drops in this drop: {1}", parts.Length, string.Join(",", parts));
        }

    }
}

