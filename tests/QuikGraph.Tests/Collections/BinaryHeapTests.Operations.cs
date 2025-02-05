using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    internal partial class BinaryHeapTests
    {
        #region Helpers

        private static void Operations<TPriority, TValue>(
            [NotNull] BinaryHeap<TPriority, TValue> heap,
            [NotNull] KeyValuePair<bool, TPriority>[] values)
        {
            foreach (KeyValuePair<bool, TPriority> value in values)
            {
                if (value.Key)
                {
                    heap.Add(value.Value, default);
                }
                else
                {
                    heap.RemoveMinimum();
                }

                AssertInvariant(heap);
            }
        }

        private static void CheckOperationsHeap(
            [NotNull] BinaryHeap<int, int> heap,
            [NotNull] KeyValuePair<bool, int>[] pairs,
            int expectedCapacity,
            int expectedCount)
        {
            Operations(heap, pairs);
            CheckHeapSizes(heap, expectedCapacity, expectedCount);
        }

        #endregion

        [Test]
        public void Operations1()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<bool, int>[0];

            CheckOperationsHeap(binaryHeap, keyValuePairs, 0, 0);
        }

        [Test]
        public void Operations2()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<bool, int>[1];
            var s0 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[0] = s0;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void Operations3()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<bool, int>[1];
            var s0 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[0] = s0;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 1, 1);
        }

        [Test]
        public void Operations4()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<bool, int>[2];
            var s0 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(default, 16384);
            keyValuePairs[1] = s1;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 1, 0);
        }

        [Test]
        public void Operations5()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(1);
            var keyValuePairs = new KeyValuePair<bool, int>[2];
            var s0 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[1] = s1;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 3, 2);
        }

        [Test]
        public void Operations6()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<bool, int>[2];
            var s0 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[1] = s1;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 2, 2);
        }

        [Test]
        public void Operations7()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<bool, int>[3];
            var s0 = new KeyValuePair<bool, int>(true, 1);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[1] = s1;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 2, 1);
        }

        [Test]
        public void Operations8()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<bool, int>[4];
            var s0 = new KeyValuePair<bool, int>(true, 1);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[1] = s1;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 2, 0);
        }

        [Test]
        public void Operations9()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<bool, int>[3];
            var s0 = new KeyValuePair<bool, int>(true, 1);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[1] = s1;
            var s2 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[2] = s2;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 5, 3);
        }

        [Test]
        public void Operations10()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
            var keyValuePairs = new KeyValuePair<bool, int>[5];
            var s0 = new KeyValuePair<bool, int>(true, 1);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[1] = s1;
            var s2 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[3] = s2;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 2, 1);
        }

        [Test]
        public void Operations11()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
            var keyValuePairs = new KeyValuePair<bool, int>[2];
            var s0 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, default);
            keyValuePairs[1] = s1;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 3, 2);
        }

        [Test]
        public void Operations12()
        {
            BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(3);
            var keyValuePairs = new KeyValuePair<bool, int>[4];
            var s0 = new KeyValuePair<bool, int>(true, -507);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<bool, int>(true, 2147483136);
            keyValuePairs[1] = s1;
            var s2 = new KeyValuePair<bool, int>(true, -507);
            keyValuePairs[2] = s2;
            var s3 = new KeyValuePair<bool, int>(default, 16384);
            keyValuePairs[3] = s3;

            CheckOperationsHeap(binaryHeap, keyValuePairs, 3, 2);
        }

        [Test]
        public void OperationsThrowsInvalidOperationException1()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
                var keyValuePairs = new KeyValuePair<bool, int>[1];

                Operations(binaryHeap, keyValuePairs);
            });
        }

        [Test]
        public void OperationsThrowsInvalidOperationException2()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
                var keyValuePairs = new KeyValuePair<bool, int>[5];
                var s0 = new KeyValuePair<bool, int>(true, 1);
                keyValuePairs[0] = s0;
                var s1 = new KeyValuePair<bool, int>(true, default);
                keyValuePairs[1] = s1;

                Operations(binaryHeap, keyValuePairs);
            });
        }

        [Test]
        public void OperationsThrowsInvalidOperationException3()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(2);
                var keyValuePairs = new KeyValuePair<bool, int>[5];
                var s0 = new KeyValuePair<bool, int>(true, -2147352575);
                keyValuePairs[0] = s0;
                var s1 = new KeyValuePair<bool, int>(true, 131072);
                keyValuePairs[1] = s1;
                Operations(binaryHeap, keyValuePairs);
            });
        }

        [Test]
        public void OperationsThrowsInvalidOperationException4()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                BinaryHeap<int, int> binaryHeap = BinaryHeapFactory.Create(0);
                var keyValuePairs = new KeyValuePair<bool, int>[5];
                var s0 = new KeyValuePair<bool, int>(true, default);
                keyValuePairs[0] = s0;
                var s1 = new KeyValuePair<bool, int>(default, 1);
                keyValuePairs[1] = s1;
                var s2 = new KeyValuePair<bool, int>(true, 1);
                keyValuePairs[2] = s2;
                var s3 = new KeyValuePair<bool, int>(default, 1);
                keyValuePairs[3] = s3;
                var s4 = new KeyValuePair<bool, int>(default, 1);
                keyValuePairs[4] = s4;
                Operations(binaryHeap, keyValuePairs);
            });
        }
    }
}
