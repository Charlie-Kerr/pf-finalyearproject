using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP
{
    internal class Drop
    {
        public int[] parts;
        public String[] data;

        public Drop(int[] parts, String[] data)
        {
            this.parts = parts;
            this.data = data;
        }

        public override string ToString()
        {
            return String.Format("Drop degree is {0}\nThe drops in this drop: {1}", parts.Length, string.Join(",", data));
        }

    }
}

