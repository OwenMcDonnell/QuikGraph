using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    internal partial class BinaryHeapTests
    {
        #region Helpers

        private static void InsertAndIndexOf<TPriority, TValue>(
            [NotNull] BinaryHeap<TPriority, TValue> heap,
            [NotNull] KeyValuePair<TPriority, TValue>[] pairs)
        {
            foreach (KeyValuePair<TPriority, TValue> pair in pairs)
                heap.Add(pair.Key, pair.Value);
            foreach (KeyValuePair<TPriority, TValue> pair in pairs)
                Assert.IsTrue(heap.IndexOf(pair.Value) > -1);
        }

        private static void CheckInsertAndIndexOfHeap(
            [NotNull] BinaryHeap<int, int> heap,
            [NotNull] KeyValuePair<int, int>[] pairs,
            int expectedCapacity,
            int expectedCount)
        {
            InsertAndIndexOf(heap, pairs);
            CheckHeapSizes(heap, expectedCapacity, expectedCount);
        }

        #endregion

        [Test]
        public void InsertAndIndexOf1()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[0];

            CheckInsertAndIndexOfHeap(binaryHeap, keyValuePairs, 0, 0);
        }

        [Test]
        public void InsertAndIndexOf2()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndIndexOfHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void InsertAndIndexOf3()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndIndexOfHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void InsertAndIndexOf4()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[2];

            CheckInsertAndIndexOfHeap(binaryHeap, keyValuePairs, 3, 2);
        }

        [Test]
        public void InsertAndIndexOf5()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(3);
            var keyValuePairs = new KeyValuePair<int, int>[2];

            CheckInsertAndIndexOfHeap(binaryHeap, keyValuePairs, 3, 2);
        }

        [Test]
        public void InsertAndIndexOf6()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<int, int>[3];
            var s0 = new KeyValuePair<int, int>(1, default);
            keyValuePairs[1] = s0;
            var s1 = new KeyValuePair<int, int>(int.MinValue, 1);
            keyValuePairs[2] = s1;

            CheckInsertAndIndexOfHeap(binaryHeap, keyValuePairs, 5, 3);
        }
    }
}
