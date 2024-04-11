using FYP;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTests
{
    [TestClass]
    public class DecoderTests
    {
        [TestMethod]
        public void testReduceMultipleDegrees() 
        { 
            string testString = "Hello World.";
            byte[] decodedParts = Encoding.ASCII.GetBytes(testString);

            FYP.Encoder encoder = new FYP.Encoder("test.txt", SolitonDistributionType.ISD);
            List<Drop> drops = encoder.GenerateDroplets((int)encoder.getByteSize());

            int[] partsToReduce = drops[0].parts.Skip(1).ToArray();
            FYP.Decoder.reduceMultipleDegrees(drops[0], decodedParts, partsToReduce);
            Assert.IsTrue(drops[0].data[0] == decodedParts[drops[0].parts[0]]);
        }

        [TestMethod]
        public void testPriorityQueue() 
        {
            string testString = "Hello World.";
            byte[] decodedParts = Encoding.ASCII.GetBytes(testString);

            FYP.Encoder encoder = new FYP.Encoder("test.txt", SolitonDistributionType.ISD);
            List<Drop> drops = encoder.GenerateDroplets((int)encoder.getByteSize());

            PriorityQueue<Drop, int> priorityQueue = new PriorityQueue<Drop, int>(drops.Count);
            FYP.Decoder.fillPriorityQueue(priorityQueue);
            List<(Drop, int)> temp; // allocate memeory to this upfront?

            for (int i = 0; i < drops.Count; i++)
            {
                if (drops[i].getDegree() > 1) 
                {
                    temp = priorityQueue.UnorderedItems.ToList();
                    temp.Remove((drops[i], drops[i].getDegree()));
                    Console.WriteLine(drops[i].getDegree());
                    drops[i].setDegree(drops[i].getDegree() - 1);
                    Console.WriteLine(drops[i].getDegree());
                    temp.Add((drops[i], drops[i].getDegree()));
                    priorityQueue = new PriorityQueue<Drop, int>(temp);
                    Assert.IsTrue(true);
                    //test used to examine the inner members of the priority queue and the drops array
                    //we need to reset the priority of the drop in the priority queue
                    break;
                }
            }
        }

        [TestMethod]
        public void testRemoveDropFromDictionary() 
        { 
            Dictionary<int, List<Drop>> dictionary = new Dictionary<int, List<Drop>>();
            FYP.Encoder encoder = new FYP.Encoder("test.txt", SolitonDistributionType.ISD);
            List<Drop> drops = encoder.GenerateDroplets((int)encoder.getByteSize() * 2);
            FYP.Decoder decoder = new FYP.Decoder(encoder.getByteSize(), drops);
            FYP.Decoder.fillDictionary(dictionary);
            List<Drop> dropsToRemove = new List<Drop>();

            foreach (Drop d in dictionary[1]) 
            {
                if (d.getDegree() > 1) 
                {
                    dropsToRemove.Add(d);
                }
            }
            foreach (Drop d in dropsToRemove)
            {
                dictionary[1].Remove(d);
            }

            foreach (Drop d in dictionary[1]) 
            {
                Assert.IsTrue(d.getDegree() == 1);
            }
        }

        [TestMethod]
        public void testImprovedRebuildPlaintext()
        {
            string testString = "Hello World.";
            byte[] decodedParts = Encoding.ASCII.GetBytes(testString);

            FYP.Encoder encoder = new FYP.Encoder("test.txt", SolitonDistributionType.ISD);
            List<Drop> drops = encoder.GenerateDroplets(2);
            FYP.Decoder decoder = new FYP.Decoder(encoder.getByteSize(), drops);
            string rebuiltPlaintext = decoder.improvedRebuildPlaintext(encoder);

            Assert.IsTrue(rebuiltPlaintext == testString);  
        }

    }
}
