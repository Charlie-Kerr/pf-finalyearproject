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
        public static DegreeDistribution Soliton;
        private static string plaintext;
        private static byte[] plainbytes;
        private static int blockSize;
        public Encoder(string path, SolitonDistributionType type)
        {
            plaintext = File.ReadAllText(path);
            plainbytes = Encoding.ASCII.GetBytes(plaintext);
            blockSize = plaintext.Length;
            if (type == SolitonDistributionType.ISD)
            {
                //Soliton = new ISD(plaintext.Length);
            }
            Soliton = new ISD(plaintext.Length);
        }
        public List<Drop> ISDGenerateDroplets()
        {
            Random rand = new Random();
            int randomPart = 0;
            int degree = 0;
            byte[] data;
            int[] parts;
            List<Drop> drops = new List<Drop>();
            HashSet<int> partsInDrop = new HashSet<int>();

            for (int i = 0; i < blockSize * 2; i++) //Creates K*1.10 drops, consider changing to variable
            {
                partsInDrop.Clear();
                degree = Soliton.next();
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
                drops.Add(new Drop(parts, encode(data, parts)));
            }
            return drops;
        }
        public static byte[] encode(byte[] data, int[] parts) //check drops that are being encoded are not the same as drops already encoded
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
