using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FYP
{
    public class Encoder
    {
        public static DegreeDistribution soliton;
        private static string plaintext;
        private static byte[] plainbytes;
        private static int blockSize;
        private static HashSet<HashSet<int>> encodedDrops = new HashSet<HashSet<int>>(HashSet<int>.CreateSetComparer());
        public Encoder(string path, SolitonDistributionType type)
        {
            plaintext = File.ReadAllText(path);
            plainbytes = Encoding.ASCII.GetBytes(plaintext);
            blockSize = plaintext.Length;
            if (type == SolitonDistributionType.ISD)
            {
                soliton = new ISD(plaintext.Length);
            }
            else if (type == SolitonDistributionType.RSD)
            {
                soliton = new RSD(plaintext.Length, 0.2, 0.05);
            }
            else 
            {
                throw new Exception("Invalid Soliton Distribution Type");
            }
        }
        public List<Drop> generateDroplets(int iterations, int chosenDegree)
        {
            Random rand = new Random();
            int randomPart = 0;
            int degree = 0;
            byte[] data;
            int[] parts;
            List<Drop> drops = new List<Drop>();
            HashSet<int> partsInDrop = new HashSet<int>();
            int count = 0;

            while(drops.Count < iterations)
            {
                partsInDrop.Clear();
                if(chosenDegree != 0)
                {
                    degree = chosenDegree;
                }
                else
                {
                    degree = soliton.next();
                }
                parts = new int[degree];
                data = new byte[degree];

                for (int j = 0; j < degree; j++)
                {
                    while (partsInDrop.Count < j + 1) //ensures that a part is not used twice in the same drop
                    {
                        randomPart = rand.Next(0, blockSize);
                        partsInDrop.Add(randomPart);
                    }
                    data[j] = plainbytes[randomPart];
                    parts[j] = randomPart;
                }
                count = encodedDrops.Count;
                encodedDrops.Add(parts.ToHashSet());
                drops.Add(new Drop(parts, encode(data, parts)));
                if(encodedDrops.Count != count + 1) //ensures that the same drop is not encoded twice
                {
                    drops.Remove(drops.Last());
                }
            }
            return drops;
        }
        public static byte[] encode(byte[] data, int[] parts)
        {
            byte result = data[0];
            for (int i = 1; i < parts.Count(); i++)
            {
                result ^= data[i];
            }
            return new byte[] { result };
        }

        public int getByteSize() {
            return plainbytes.Length;
        }
    }
}
