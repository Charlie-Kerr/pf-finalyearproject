using FYP;
using System;
using System.Text;

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

        [TestMethod]
        public void TestDictionary() 
        { 
            Dictionary<int, List<int[]>> dictionary = new Dictionary<int, List<int[]>>();
            int[] array1 = {1,2,3,4,5};
            int[] array2 = {5,2,3,4,1};

            dictionary[array1.Sum()] = new List<int[]>();
            dictionary[array1.Sum()].Add(array1);

            if (dictionary.ContainsKey(array2.Sum()))
            {
                foreach (int[] dropletParts in dictionary[array2.Sum()])
                {
                    if (array2.Length == dropletParts.Length && !array2.ToHashSet().IsSubsetOf(dropletParts))
                    {
                        dictionary[array2.Sum()].Add(array2);
                    }
                }
            }
            else
            {
                dictionary[array2.Sum()] = new List<int[]>();
                dictionary[array2.Sum()].Add(array2);
            }

            Assert.IsTrue(dictionary[array1.Sum()].Count == 1);
        }

        [TestMethod]
        public void TestDictionaryHashSet() 
        {
            string testString = "He";
            byte testByte = Encoding.ASCII.GetBytes(testString)[0];
            FYP.Decoder decoder = new FYP.Decoder();
            List<Drop> drops = ISDGenerateDroplets(Encoding.ASCII.GetBytes(testString), testString.Length);
            Dictionary<int, List<Drop>> dictionary = new Dictionary<int, List<Drop>>();

            foreach (Drop drop in drops)
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
            for (int i = 0; i < dictionary[0].Count(); i++)
            {
                decoder.reduceDegree(dictionary[0][i], testByte, 0);
            }
            foreach (KeyValuePair<int, List<Drop>> pair in dictionary)
            {
                Console.WriteLine(pair.Key);
                foreach (Drop drop in pair.Value)
                {
                    Console.WriteLine(drop.ToString());
                }
            }
        }

        [TestMethod]
        public void TestHashSet()
        {
            //use CreateSetComparer to compare the values of the sets instead of the default which compares the objects of the sets themseleves
            HashSet<HashSet<int>> hashSet = new HashSet<HashSet<int>>(HashSet<int>.CreateSetComparer());
            int[] array1 = new int[] { 1, 2, 3, 4, 5 };
            int[] array2 = new int[] { 5, 2, 3, 4, 1 };

            hashSet.Add(array1.ToHashSet());
            hashSet.Add(array2.ToHashSet());

            Assert.IsTrue(hashSet.Count == 1);
        }

        static List<Drop> ISDGenerateDroplets(byte[] plain, int size)
        {
            Random rand = new Random();
            ISD isd = new ISD(size);
            FYP.Encoder encoder = new FYP.Encoder();
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