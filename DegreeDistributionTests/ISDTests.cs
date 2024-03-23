using FYP;

namespace FYPTests
{
    [TestClass]
    public class ISDTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            ISD isd = new ISD(100);

            Assert.AreEqual(1, isd.getDegreeOfWeight(0));
            Assert.AreEqual(3, isd.getDegreeOfWeight(2));
            Assert.AreEqual(100, isd.getDegreeOfWeight(99));
        }
    }
}