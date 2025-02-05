using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    internal partial class BinaryHeapTests
    {
        #region Helpers

        private static void Insert<TPriority, TValue>(
            [NotNull] BinaryHeap<TPriority, TValue> heap,
            [NotNull] KeyValuePair<TPriority, TValue>[] pairs)
        {
            int count = heap.Count;
            foreach (KeyValuePair<TPriority, TValue> pair in pairs)
            {
                heap.Add(pair.Key, pair.Value);
                AssertInvariant(heap);
            }

            Assert.AreEqual(heap.Count, count + pairs.Length);
        }

        private static void CheckInsertHeap(
            [NotNull] BinaryHeap<int, int> heap,
            [NotNull] KeyValuePair<int, int>[] pairs,
            int expectedCapacity,
            int expectedCount)
        {
            Insert(heap, pairs);
            CheckHeapSizes(heap, expectedCapacity, expectedCount);
        }

        #endregion

        [Test]
        public void Insert1()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[0];

            CheckInsertHeap(binaryHeap, keyValuePairs, 0, 0);
        }

        [Test]
        public void Insert2()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void Insert3()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void Insert4()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[2];

            CheckInsertHeap(binaryHeap, keyValuePairs, 3, 2);
        }

        [Test]
        public void Insert5()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<int, int>[2];

            CheckInsertHeap(binaryHeap, keyValuePairs, 2, 2);
        }

        [Test]
        public void Insert6()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<int, int>[6];
            var s0 = new KeyValuePair<int, int>(1, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(1073741824, default);
            keyValuePairs[2] = s1;
            var s2 = new KeyValuePair<int, int>(int.MinValue, default);
            keyValuePairs[3] = s2;
            var s3 = new KeyValuePair<int, int>(int.MinValue, 1);
            keyValuePairs[4] = s3;
            var s4 = new KeyValuePair<int, int>(11, 11);
            keyValuePairs[5] = s4;

            CheckInsertHeap(binaryHeap, keyValuePairs, 11, 6);
        }
    }
}
