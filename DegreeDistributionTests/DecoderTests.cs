using FYP;
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
            //FYP.Decoder decoder = new FYP.Decoder(encoder.getByteSize(), drops);

            int[] partsToReduce = drops[0].parts.Skip(1).ToArray();
            FYP.Decoder.reduceMultipleDegrees(drops[0], decodedParts, partsToReduce);
            Assert.IsTrue(drops[0].data[0] == decodedParts[drops[0].parts[0]]);
        }

        [TestMethod]
        public void testPriorityQueue() 
        { 
        
        }

    }
}
