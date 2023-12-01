using System.Globalization;
using System.Text;

namespace FYP
{
    internal class Program
    {
        public const int degree = 5; // will be attribute of encoder class
        static void Main(string[] args)
        {
            String[] data = {"First", "House", "Mouse", "Shelf", "Books"};
            string plain = "This text is a test of the encoding and decoding system.";
            List<Drop> drops = generateDroplets(Encoding.ASCII.GetBytes(plain));
            //test decode and rebuilding plaintext functions by printing decoded text to console, 
            Console.WriteLine(rebuildPlaintext(drops, Encoding.ASCII.GetByteCount(plain)));
            

        }

        static byte[] encode(byte[] data, int[] parts)
        {
            byte result = data[0];
            for (int i = 1; i < parts.Count(); i++)
            {
                result ^= data[i];
            }
            return new byte[] { result };
        }

        static byte[] decode(Drop drop) 
        {
            return null;
        }

        static string rebuildPlaintext(List<Drop> goblet, int byteSize) 
        {
            List<Drop> decodeBucket = new List<Drop>();
            byte[] decoded = new byte[byteSize];
            List<int> parts = new List<int>();
            int dCount = 0;

            foreach (Drop drop in goblet)
            {
                dCount = 0;
                if (drop.parts.Length == 1 && decoded[drop.parts[0]] == 0)
                {
                    if (decoded[drop.parts[0]] == 0) 
                    { 
                        decoded[drop.parts[0]] = drop.data[0];
                        parts.Add(drop.parts[0]);
                    }
                    //else we discard the drop from the goblet, we already have a solution for it
                }
                else
                {
                    //multi-part drops
                    for (int i = 0; i < drop.parts.Length; i++) 
                    { 
                        
                    }
                    
                }

            }
            return null;
        }

        static List<Drop> generateDroplets(byte[] plain) 
        {
            Random rand = new Random();
            byte[] data = new byte[degree];
            int dropletDegree;
            int[] parts;
            List<Drop> drops = new List<Drop>();


            //this first loop is arbitrary and will be replaced by broadcasting method
            for (int i = 0; i < plain.Length * 2; i++)
            {
                dropletDegree = getDegree();
                parts = new int[dropletDegree];
                //first drop should be degree 1 to make sure decoding process can happen
                if (i == 0)
                {
                    dropletDegree = 1;
                }

                //second loop is where droplet generation happens
                for (int j = 0; j < dropletDegree; j++)
                {
                    if (j == 0)
                    {
                        data[0] = (plain[rand.Next(0, plain.Length)]);
                        parts[0] = Array.IndexOf(plain, data[0]);
                    }
                    else
                    {
                        data[j] = plain[rand.Next(0, plain.Length)];
                        parts[j] = Array.IndexOf(plain, data[j]);
                    }
                }
                drops.Add(new Drop(parts, encode(data, parts)));
                Console.WriteLine(drops.Last().ToString());
            }
            return drops;
        }

        static int getDegree() {
            int[] probabilities = { 50, 30, 15, 5, 1 };
            Random rand = new Random();

            int i = 0;
            int degree = rand.Next(1, 101);

            foreach (int p in probabilities)
            {
                if (degree >= p)
                {
                    degree = Array.IndexOf(probabilities, p) + 1;
                    return degree;
                }
            }
            return -1; // a run should never reach here
        }
    }
}