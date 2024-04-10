using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP
{
    public class Decoder
    {
        private int byteSize;
        private int chunkSize;
        public Decoder(int byteSize)
        {
            this.byteSize = byteSize;
        }
        public string RebuildPlaintext(List<Drop> goblet, Encoder encoder)
        {
            byte[] decoded = new byte[byteSize];
            List<int> parts = new List<int>();
            bool allSolutionsFound = false;
            byte nullValue = 0;

            while (allSolutionsFound == false)
            {
                foreach (Drop drop in goblet)
                {
                    if (drop.parts.Length == 1) //consider using more efficient search to find drops of degree 1
                    {
                        if (decoded[drop.parts[0]] == 0) //more efficient that .Contains, goes straight to the index
                        {
                            decoded[drop.parts[0]] = drop.data[0];
                            parts.Add(drop.parts[0]);
                            Console.WriteLine("Part " + drop.parts[0] + " has been decoded: [" + parts.Count + "/" + byteSize + " ]");

                            //decrease the degree of all the drops by 1 that contain the part that has been decoded
                            foreach (Drop d in goblet) //consider recursive function to decrease degree
                            {
                                if (d.parts.Contains(drop.parts[0]) && d.parts.Length > 1)
                                {
                                    reduceDegree(d, drop.data[0], drop.parts[0]);
                                }
                            }
                        }
                        //else we discard the drop from the goblet, we already have a solution for it
                    }

                    //checks if we have decoded the data
                    if (!decoded.Contains(nullValue))//need to check validity of data here, not matching original message
                    {
                        allSolutionsFound = true;
                        break;
                    }
                }
            }
            //returns the decoded data in a string format when every byte has been decoded
            return Encoding.ASCII.GetString(decoded);
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

        public static void reduceDegree(Drop drop, byte decodedPart, int partToReduce)
        {
            //Creates a new parts array without the part that is being reduced, and XORs the data with the part being reduced
            drop.parts = drop.parts.Where(val => val != partToReduce).ToArray();
            drop.data[0] = drop.data[0] ^= decodedPart;
        }

        static Drop requestDrop(Encoder encoder) 
        { 
            return encoder.GenerateDroplets(1)[0];
        }
    }
}
