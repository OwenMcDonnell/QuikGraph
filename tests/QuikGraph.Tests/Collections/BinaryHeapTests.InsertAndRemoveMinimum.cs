using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    internal partial class BinaryHeapTests
    {
        #region Helpers

        private static void InsertAndRemoveMinimum<TPriority, TValue>(
            [NotNull] BinaryHeap<TPriority, TValue> heap,
            [NotNull] KeyValuePair<TPriority, TValue>[] pairs)
        {
            foreach (KeyValuePair<TPriority, TValue> pair in pairs)
                heap.Add(pair.Key, pair.Value);

            TPriority minimum = default;
            for (int i = 0; i < pairs.Length; ++i)
            {
                if (i == 0)
                {
                    minimum = heap.RemoveMinimum().Key;
                }
                else
                {
                    TPriority min = heap.RemoveMinimum().Key;
                    Assert.IsTrue(heap.PriorityComparison(minimum, min) <= 0);
                    minimum = min;
                }

                AssertInvariant(heap);
            }

            Assert.AreEqual(0, heap.Count);
        }

        private static void CheckInsertAndRemoveMinimumHeap(
            [NotNull] BinaryHeap<int, int> heap,
            [NotNull] KeyValuePair<int, int>[] pairs,
            int expectedCapacity,
            int expectedCount)
        {
            InsertAndRemoveMinimum(heap, pairs);
            CheckHeapSizes(heap, expectedCapacity, expectedCount);
        }

        #endregion

        [Test]
        public void InsertAndRemoveMinimum1()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[0];

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 0, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum2()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 1, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum3()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 1, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum4()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(1, default);
            keyValuePairs[1] = s0;

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 3, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum5()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(1, default);
            keyValuePairs[1] = s0;

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 2, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum6()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(1, default);
            keyValuePairs[1] = s1;

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 3, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum7()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(3);
            var keyValuePairs = new KeyValuePair<int, int>[3];
            var s0 = new KeyValuePair<int, int>(-2147483647, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[2] = s1;

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 3, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum8()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(3);
            var keyValuePairs = new KeyValuePair<int, int>[3];

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 3, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum9()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[4];
            var s0 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(2, default);
            keyValuePairs[2] = s1;
            var s2 = new KeyValuePair<int, int>(3, default);
            keyValuePairs[3] = s2;

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 7, 0);
        }

        [Test]
        public void InsertAndRemoveMinimum10()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[4];
            var s0 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(2, default);
            keyValuePairs[2] = s1;
            var s2 = new KeyValuePair<int, int>(1, default);
            keyValuePairs[3] = s2;

            CheckInsertAndRemoveMinimumHeap(binaryHeap, keyValuePairs, 7, 0);
        }
    }
}