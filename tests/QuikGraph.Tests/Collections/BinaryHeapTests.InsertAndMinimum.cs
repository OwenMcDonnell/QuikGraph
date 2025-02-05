using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    internal partial class BinaryHeapTests
    {
        #region Helpers

        private static void InsertAndMinimum<TPriority, TValue>(
            [NotNull] BinaryHeap<TPriority, TValue> target,
            [NotNull] KeyValuePair<TPriority, TValue>[] pairs)
        {
            Assert.IsTrue(pairs.Length > 0);

            TPriority minimum = default;
            for (int i = 0; i < pairs.Length; ++i)
            {
                KeyValuePair<TPriority, TValue> pair = pairs[i];
                if (i == 0)
                    minimum = pair.Key;
                else
                    minimum = target.PriorityComparison(pair.Key, minimum) < 0 ? pair.Key : minimum;
                target.Add(pair.Key, pair.Value);
                // Check minimum
                KeyValuePair<TPriority, TValue> pairMin = target.Minimum();
                Assert.AreEqual(minimum, pairMin.Key);
            }

            AssertInvariant(target);
        }

        private static void CheckInsertAndMinimumHeap(
            [NotNull] BinaryHeap<int, int> heap,
            [NotNull] KeyValuePair<int, int>[] pairs,
            int expectedCapacity,
            int expectedCount)
        {
            InsertAndMinimum(heap, pairs);
            CheckHeapSizes(heap, expectedCapacity, expectedCount);
        }

        #endregion

        [Test]
        public void InsertAndMinimum1()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                1,
                1);
        }

        [Test]
        public void InsertAndMinimum2()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                1,
                1);
        }

        [Test]
        public void InsertAndMinimum3()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[1] = s0;

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                3,
                2);
        }

        [Test]
        public void InsertAndMinimum4()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(3);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[1] = s0;

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                3,
                2);
        }

        [Test]
        public void InsertAndMinimum5()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(16384, 16384);
            keyValuePairs[1] = s0;

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                2,
                2);
        }

        [Test]
        public void InsertAndMinimum6()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<int, int>[3];
            var s0 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[2] = s0;

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                5,
                3);
        }

        [Test]
        public void InsertAndMinimum7()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(3);
            var keyValuePairs = new KeyValuePair<int, int>[3];
            var s0 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(-2147483647, default);
            keyValuePairs[2] = s1;

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                3,
                3);
        }

        [Test]
        public void InsertAndMinimum8()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(-63, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(-63, default);
            keyValuePairs[1] = s1;

            CheckInsertAndMinimumHeap(
                binaryHeap,
                keyValuePairs,
                3,
                2);
        }
    }
}
