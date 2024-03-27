using FYP;

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
    }
}