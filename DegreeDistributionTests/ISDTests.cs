using FYP;

namespace FYPTests
{
    [TestClass]
    public class ISDTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            ISD isd = new ISD(100, 1000000001);

            Assert.AreEqual(1, isd.next());
        }
    }
}