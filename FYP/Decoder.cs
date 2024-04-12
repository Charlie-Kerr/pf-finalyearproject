using System;
using System.Collections;
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
        private static List<Drop> goblet = new List<Drop>();
        public Decoder(int byteSize, List<Drop> inputDecode)
        {
            this.byteSize = byteSize;
            goblet = new List<Drop>(inputDecode);
        }

        public int getGobletSize()
        {
            return goblet.Count;
        }

        public string improvedRebuildPlaintext(Encoder encoder)
        {
            //Contains dictionary which looks up by part number to find all drops containing that part
            Dictionary<int, List<Drop>> dictionary = new Dictionary<int, List<Drop>>();
            fillDictionary(dictionary);

            //Priority queue to store drops in order of degree
            PriorityQueue<Drop, int> decodeQueue = new PriorityQueue<Drop, int>(goblet.Count);
            fillPriorityQueue(decodeQueue);
            List<(Drop, int)> unorderedDecodeQueue = decodeQueue.UnorderedItems.ToList(); //used to reset priorities

            byte[] decoded = new byte[byteSize]; //contains all decoded parts data
            int decodeCount = 0;
            bool allSolutionsFound = false;
            byte nullValue = 0;
            Drop currentDrop;
            int nullPriority = 0;
            List<Drop> dropsToRemoveFromDictionary = new List<Drop>();

            while (allSolutionsFound == false)
            {
                unorderedDecodeQueue = decodeQueue.UnorderedItems.ToList();//resets unorderedDecodeQueue
                nullPriority = 0;
                if (decodeQueue.TryPeek(out currentDrop, out nullPriority) && nullPriority == 1) //decodeQueue.Peek().getDegree() == 1
                {
                    currentDrop = decodeQueue.Dequeue();
                    if (decoded[currentDrop.parts[0]] == 0)
                    {
                        decoded[currentDrop.parts[0]] = currentDrop.data[0];
                        decodeCount++;
                        Console.WriteLine("Part " + currentDrop.parts[0] + " has been decoded: [" + decodeCount + "/" + byteSize + "]");

                        foreach (Drop d in dictionary[currentDrop.parts[0]])
                        {
                            if (d.getDegree() > 1)
                            {
                                unorderedDecodeQueue.Remove((d, d.getDegree()));
                                reduceDegree(d, currentDrop.data[0], currentDrop.parts[0]);
                                unorderedDecodeQueue.Add((d, d.getDegree()));
                                dropsToRemoveFromDictionary.Add(d);
                            }
                        }
                        foreach (Drop d in dropsToRemoveFromDictionary) //removes the drops from the dictionary
                        {
                            dictionary[currentDrop.parts[0]].Remove(d);
                        }
                        decodeQueue = new PriorityQueue<Drop, int>(unorderedDecodeQueue);//resets the priority queue with updated values
                    }
                }
                else 
                { 
                    currentDrop = requestDrop(encoder);
                    if (isDropDecoded(currentDrop, decoded) == false) 
                    {
                        reduceMultipleDegrees(currentDrop, decoded, currentDrop.parts);//reduces by as many parts as possible
                        decodeQueue.Enqueue(currentDrop, currentDrop.getDegree());
                        addDropToDictionary(dictionary, currentDrop);
                        goblet.Add(currentDrop);//added to goblet to keep track of number of drops used to decode
                    }
                    //else: if all parts in the drop are decoded, program will loop and request again
                }

                //checks if we have decoded the data
                if (!decoded.Contains(nullValue))
                {
                    allSolutionsFound = true;
                    break;
                }
            }
            //returns the decoded data in a string format when every byte has been decoded
            return Encoding.ASCII.GetString(decoded);
        }

        static bool isDropDecoded(Drop drop, byte[] decoded)
        {
            for (int i = 0; i < drop.parts.Length; i++)
            {
                if (decoded[drop.parts[i]] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static void fillPriorityQueue(PriorityQueue<Drop, int> decodeQueue)
        {
            foreach (Drop drop in goblet)
            {
                decodeQueue.Enqueue(drop, drop.getDegree());
            }
        }

        public static void fillDictionary(Dictionary<int, List<Drop>> dictionary) 
        {
            foreach (Drop drop in goblet)
            {
                for (int i = 0; i < drop.parts.Length; i++)
                {
                    if (dictionary.ContainsKey(drop.parts[i]))
                    {
                        dictionary[drop.parts[i]].Add(drop);
                    }
                    else
                    {
                        dictionary[drop.parts[i]] = new List<Drop>();
                        dictionary[drop.parts[i]].Add(drop);
                    }
                }
            }
        }

        static void addDropToDictionary(Dictionary<int, List<Drop>> dictionary, Drop drop)
        {
            for (int i = 0; i < drop.parts.Length; i++)
            {
                if (dictionary.ContainsKey(drop.parts[i]))
                {
                    dictionary[drop.parts[i]].Add(drop);
                }
                else
                {
                    dictionary[drop.parts[i]] = new List<Drop>();
                    dictionary[drop.parts[i]].Add(drop);
                }
            }
        }

        public static void reduceMultipleDegrees(Drop drop, byte[] decodedParts, int[] partsToReduce)
        {
            foreach (int part in partsToReduce)
            {
                if (decodedParts[part] != 0)
                {
                    reduceDegree(drop, decodedParts[part], part);
                }
            }
        }
        public static void reduceDegree(Drop drop, byte decodedPart, int partToReduce)
        {
            //Creates a new parts array without the part that is being reduced, and XORs the data with the part being reduced
            drop.parts = drop.parts.Where(val => val != partToReduce).ToArray();
            drop.data[0] = drop.data[0] ^= decodedPart;
            drop.setDegree(drop.parts.Length);//length of new parts array
        }

        static Drop requestDrop(Encoder encoder) 
        {
            Console.WriteLine("Requested a new drop");
            return encoder.generateDroplets(1,0)[0];
        }
    }
}
