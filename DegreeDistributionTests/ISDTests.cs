using FYP;
using System;

namespace FYPTests
{
    [TestClass]
    public class ISDTests
    {
        [TestMethod]
        public void TestBinarySearch()
        {
            ISD isd = new ISD(100);

            Assert.AreEqual(1, isd.getDegreeOfWeight(0));
            Assert.AreEqual(2, isd.getDegreeOfWeight(1));
            Assert.AreEqual(3, isd.getDegreeOfWeight(2));
            Assert.AreEqual(100, isd.getDegreeOfWeight(99));
        }

        [TestMethod]
        public void TestNext()
        {
            ISD isd = new ISD(100, 100);

            Assert.IsTrue(isd.next() >= 1 && isd.next() <= 100);
        }

        [TestMethod]
        public void TestISDGenerateDroplets()
        {
            ISD isd = new ISD(5000, 100);
            byte[] data = new byte[5000];
            List<Drop> drops = ISDGenerateDroplets(data, 5000);

            foreach (Drop drop in drops)
            {
                Assert.IsTrue(drop.parts.Length >= 1 && drop.parts.Length <= 5000);
                if (drop.parts.ToHashSet().Count() != drop.parts.Length) 
                {
                    Console.WriteLine("Set: " + drop.parts.ToHashSet().Count() + " original length: " + drop.parts.Length);
                    List<int> distinctNumbers = drop.parts.Distinct().ToList();
                    List<int> repeatedNumbers = drop.parts.ToList();
                    foreach (int number in distinctNumbers)
                    {
                        repeatedNumbers.Remove(number);
                    }
                    Console.WriteLine(repeatedNumbers[0]);
                    Assert.IsTrue(drop.parts.ToHashSet().Count() == drop.parts.Length);
                }
            }
        }

        static List<Drop> ISDGenerateDroplets(byte[] plain, int size)
        {
            Random rand = new Random();
            ISD isd = new ISD(size);
            Encoder encoder = new Encoder();
            int randomPart = 0;
            int degree = 0;
            byte[] data;
            int[] parts;
            List<Drop> drops = new List<Drop>();
            HashSet<int> partsInDrop = new HashSet<int>();

            for (int i = 0; i < size * 1.10; i++) //Creates K*1.10 drops, consider changing to variable - potentially externalise loop
            {
                partsInDrop.Clear();
                degree = isd.next();
                parts = new int[degree];
                data = new byte[degree];

                for (int j = 0; j < degree; j++)
                {
                    while (partsInDrop.Count < j+1) //ensures that a part is not used twice in the same drop
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
    }
}