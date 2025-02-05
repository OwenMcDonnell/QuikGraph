using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    internal partial class BinaryHeapTests
    {
        #region Helpers

        private static void InsertAndEnumerate<TPriority, TValue>(
            [NotNull] BinaryHeap<TPriority, TValue> heap,
            [NotNull] KeyValuePair<TPriority, TValue>[] pairs)
        {
            var dictionary = new Dictionary<TPriority, TValue>();
            foreach (KeyValuePair<TPriority, TValue> pair in pairs)
            {
                heap.Add(pair.Key, pair.Value);
                dictionary[pair.Key] = pair.Value;
            }

            QuikGraphAssert.TrueForAll(heap, pair => dictionary.ContainsKey(pair.Key));
        }

        private static void CheckInsertAndEnumerateHeap(
            [NotNull] BinaryHeap<int, int> heap,
            [NotNull] KeyValuePair<int, int>[] pairs,
            int expectedCapacity,
            int expectedCount)
        {
            InsertAndEnumerate(heap, pairs);
            CheckHeapSizes(heap, expectedCapacity, expectedCount);
        }

        #endregion

        [Test]
        public void InsertAndEnumerate1()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[0];

            CheckInsertAndEnumerateHeap(binaryHeap, keyValuePairs, 0, 0);
        }

        [Test]
        public void InsertAndEnumerate2()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndEnumerateHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void InsertAndEnumerate3()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<int, int>[1];

            CheckInsertAndEnumerateHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void InsertAndEnumerate4()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(414277560, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(16384, 16384);
            keyValuePairs[1] = s1;

            CheckInsertAndEnumerateHeap(binaryHeap, keyValuePairs, 3, 2);
        }

        [Test]
        public void InsertAndEnumerate5()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(995098625, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(16384, 16384);
            keyValuePairs[1] = s1;

            CheckInsertAndEnumerateHeap(binaryHeap, keyValuePairs, 2, 2);
        }
    }
}
