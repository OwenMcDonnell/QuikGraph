using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="ForestDisjointSet{T}"/>.
    /// </summary>
    [TestFixture]
    internal class ForestDisjointSetTests
    {
        private void Unions(int elementCount, [NotNull] KeyValuePair<int,int>[] unions)
        {
            Assert.IsTrue(0 < elementCount);
            QuikGraphAssert.TrueForAll(
                unions, 
                u => 0 <= u.Key 
                     && u.Key < elementCount 
                     && 0 <= u.Value 
                     && u.Value < elementCount);

            var target = new ForestDisjointSet<int>();
            // Fill up with 0..elementCount - 1
            for (int i = 0; i < elementCount; ++i)
            {
                target.MakeSet(i);
                Assert.IsTrue(target.Contains(i));
                Assert.AreEqual(i + 1, target.ElementCount);
                Assert.AreEqual(i + 1, target.SetCount);
            }

            // Apply Union for pairs unions[i], unions[i+1]
            foreach (KeyValuePair<int, int> pair in unions)
            {
                int left = pair.Key;
                int right= pair.Value;

                int setCount = target.SetCount;
                bool unioned = target.Union(left, right);
                
                // Should be in the same set now
                Assert.IsTrue(target.AreInSameSet(left, right));
                
                // If unioned, the count decreased by 1
                QuikGraphAssert.ImpliesIsTrue(unioned, () => setCount - 1 == target.SetCount);
            }
        }

        // TODO: Add real tests.
    }
}
