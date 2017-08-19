using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Superbest_random;

namespace Tests
{
    [TestClass]
    public class PermutationTest
    {
        [TestMethod]
        public void TestPermutations()
        {
            var r = new Random();

            const int n = 1000000;
            const int k = 1000;
            const int iterations = 100;

            for (var i = 0; i < iterations; i++)
            {
                var p = r.Permutation(n, k);
                Assert.AreEqual(k, p.Count());
                Assert.AreEqual(k, p.Distinct().Count());
            }
        }
    }
}
