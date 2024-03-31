using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP
{
    public class Decoder
    {
        public Decoder()
        {

        }
        public byte decode(Drop drop, byte[] decodedParts, int partToDecode)
        {
            byte result = drop.data[0];
            for (int i = 0; i < drop.parts.Length; i++)
            {
                if (i != partToDecode)
                {
                    //XORs the byte to decode with all the parts that were used to encode it
                    result ^= decodedParts[drop.parts[i]];
                }
            }
            return result;
        }

        public void reduceDegree(Drop drop, byte decodedPart, int partToReduce)
        {
            //Creates a new parts array without the part that is being reduced, and XORs the data with the part being reduced
            drop.parts = drop.parts.Where(val => val != partToReduce).ToArray();
            drop.data[0] = drop.data[0] ^= decodedPart;
        }
    }
}
