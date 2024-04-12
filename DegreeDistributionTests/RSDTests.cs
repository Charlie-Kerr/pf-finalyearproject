using FYP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTests
{
    [TestClass]
    public class RSDTests
    {
        [TestMethod]
        public void testNext()
        {
            RSD rsd = new RSD(100, 0.5, 0.1);
            int next = 0;
            for (int i = 0; i < 100; i++)
            {
                next = rsd.next();
                Assert.IsTrue(next >= 0 && next <= 100);
                Console.WriteLine(next);
            }
        }

    }
}
