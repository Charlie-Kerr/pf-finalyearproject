using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP
{
    internal class Encoder
    {
        public Encoder()
        {

        }
        public byte[] encode(byte[] data, int[] parts) //check drops that are being encoded are not the same as drops already encoded
        {
            byte result = data[0];
            for (int i = 1; i < parts.Count(); i++)
            {
                result ^= data[i];
            }
            return new byte[] { result };
        }
    }
}
