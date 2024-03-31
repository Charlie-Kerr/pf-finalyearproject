using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FYP
{
    internal class Program
    {
        public const int degree = 5; // will be attribute of encoder class

        public static Encoder encoder = new Encoder();
        public static Decoder decoder = new Decoder();
        public static ISD isd; //consider making this an attribute of the encoder class?
        static void Main(string[] args)
        {
            string longerPlain = File.ReadAllText("text.txt"); //from bin\debug\net6.0\text.txt
            isd = new ISD(longerPlain.Length);//intiate ISD with size of data

            var watch = System.Diagnostics.Stopwatch.StartNew();
            //List<Drop> drops = generateDroplets(Encoding.ASCII.GetBytes(longerPlain));
            List<Drop> drops = ISDGenerateDroplets(Encoding.ASCII.GetBytes(longerPlain), longerPlain.Length);
            watch.Stop();
            var generateTime = watch.ElapsedMilliseconds;
            //testEncodedParts(longerPlain, drops);

            watch.Restart();
            //test decode and rebuilding plaintext functions by printing decoded text to console
            //Console.WriteLine(rebuildPlaintext(drops, Encoding.ASCII.GetByteCount(longerPlain)));
            Console.WriteLine(ISDRebuildPlaintext(drops, Encoding.ASCII.GetByteCount(longerPlain)));
            watch.Stop();
            var totalDecodetime = watch.ElapsedMilliseconds;
            Console.WriteLine("Time taken to generate: " + generateTime + "\nTime taken to decode: " + totalDecodetime);
            
        }

        static void testEncodedParts(string longerPlain, List<Drop> drops) 
        {
            int[] encodedParts = new int[Encoding.ASCII.GetByteCount(longerPlain)];
            foreach (Drop drop in drops)
            {
                foreach (int part in drop.parts)
                {
                    encodedParts[part]++;
                }
            }
            for (int i = 0; i < encodedParts.Length; i++)
            {
                if (encodedParts[i] == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }

        static string ISDRebuildPlaintext(List<Drop> goblet, int byteSize) 
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
                                    decoder.reduceDegree(d, drop.data[0], drop.parts[0]);
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
        static string rebuildPlaintext(List<Drop> goblet, int byteSize) 
        {
            List<Drop> decodeBucket = new List<Drop>();
            byte[] decoded = new byte[byteSize];
            List<int> parts = new List<int>();
            int dCount = 0;
            int dPosition = 0;
            bool allSolutionsFound = false;
            byte nullValue = 0;

            while (allSolutionsFound == false)
            {
                foreach (Drop drop in goblet)
                {
                    dCount = 0;
                    if (drop.parts.Length == 1)
                    {
                        if (decoded[drop.parts[0]] == 0)
                        {
                            decoded[drop.parts[0]] = drop.data[0];
                            parts.Add(drop.parts[0]);
                            Console.WriteLine("Part " + drop.parts[0] + " has been decoded: [" + parts.Count + "/" + byteSize + " ]");
                        }
                        //else we discard the drop from the goblet, we already have a solution for it
                    }
                    else if (drop.parts.ToHashSet().IsSubsetOf(parts.ToHashSet()))
                    {
                        //else we discard the drop from the goblet, we already have the solutions for all the parts that are in the drop
                    }
                    else
                    {
                        //for drops degree > 1
                        for (int i = 0; i < drop.parts.Length; i++)
                        {
                            if (parts.Contains(drop.parts[i]))
                            {
                                dCount++;
                            }
                            else
                            {
                                dPosition = i;
                            }
                        }

                        if (dCount == drop.parts.Length - 1)
                        {
                            decoded[drop.parts[dPosition]] = decoder.decode(drop, decoded, dPosition); //consider parsing just the required bytes to decode the drop
                            parts.Add(drop.parts[dPosition]);
                            Console.WriteLine("Part " + drop.parts[dPosition] + " has been decoded: [" + parts.Count + "/" + byteSize + " ]");
                        }
                    }

                    //checks if we have decoded the data
                    if (!decoded.Contains(nullValue))
                    { 
                        allSolutionsFound = true;
                        break;
                    }

                }
            }
            //returns the decoded data in a string format when every byte has been decoded
            return Encoding.ASCII.GetString(decoded);
        }

        static List<Drop> ISDGenerateDroplets(byte[] plain, int size)
        {
            Random rand = new Random();
            int randomPart = 0;
            int degree = 0;
            byte[] data;
            int[] parts;
            List<Drop> drops = new List<Drop>();
            HashSet<int> partsInDrop = new HashSet<int>();

            for (int i = 0; i < size * 2; i++) //Creates K*1.10 drops, consider changing to variable
            {
                partsInDrop.Clear();
                degree = isd.next();
                parts = new int[degree];
                data = new byte[degree];

                for (int j = 0; j < degree; j++)
                {
                    while (partsInDrop.Count < j + 1) //ensures that a part is not used twice in the same drop
                    {
                        randomPart = rand.Next(0, size);
                        partsInDrop.Add(randomPart);
                    }
                    data[j] = plain[randomPart];
                    parts[j] = randomPart;
                }
                drops.Add(new Drop(parts, encoder.encode(data, parts)));
            }
            return drops;
        }

        static List<Drop> generateDroplets(byte[] plain) 
        {
            Random rand = new Random();
            int randPart = 0;
            byte[] data = new byte[degree];
            int dropletDegree;
            int[] parts;
            List<Drop> drops = new List<Drop>();


            //this first loop is arbitrary and will be replaced by broadcasting method
            for (int i = 0; i < plain.Length * 5; i++)
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
                    randPart = rand.Next(0, plain.Length);
                    if (j == 0)
                    {
                        data[0] = plain[randPart];
                        parts[0] = randPart;
                    }
                    else
                    {
                        data[j] = plain[randPart];
                        parts[j] = randPart;
                    }
                }
                drops.Add(new Drop(parts, encoder.encode(data, parts)));

            }
            return drops;
        }

        static int getDegree() {
            int[] probabilities = { 50, 30, 15, 5, 1 };
            Random rand = new Random();

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