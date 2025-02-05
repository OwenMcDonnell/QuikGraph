using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace QuikGraph.Collections
{
    /// <summary>
    /// Priority queue to sort vertices by distance priority (use <see cref="BinaryHeap{TPriority,TValue}"/>).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TDistance">Distance type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class BinaryQueue<TVertex, TDistance> : IPriorityQueue<TVertex>
    {
        [NotNull]
        private readonly Func<TVertex, TDistance> _distanceFunc;

        [NotNull]
        private readonly BinaryHeap<TDistance, TVertex> _heap;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryQueue{TVertex,TDistance}"/> class.
        /// </summary>
        /// <param name="distanceFunc">Function that compute the distance for a given vertex.</param>
        public BinaryQueue([NotNull] Func<TVertex, TDistance> distanceFunc)
            : this(distanceFunc, Comparer<TDistance>.Default.Compare)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryQueue{TVertex,TDistance}"/> class.
        /// </summary>
        /// <param name="distanceFunc">Function that compute the distance for a given vertex.</param>
        /// <param name="distanceComparison">Comparer of distances.</param>
        public BinaryQueue(
            [NotNull] Func<TVertex, TDistance> distanceFunc, 
            [NotNull] Comparison<TDistance> distanceComparison)
        {
            if (distanceFunc is null)
                throw new ArgumentNullException(nameof(distanceFunc));
            if (distanceComparison is null)
                throw new ArgumentNullException(nameof(distanceComparison));

            _distanceFunc = distanceFunc;
            _heap = new BinaryHeap<TDistance, TVertex>(distanceComparison);
        }

        #region IQueue<TVertex>

        /// <inheritdoc />
        public int Count => _heap.Count;

        /// <inheritdoc />
        public bool Contains(TVertex value)
        {
            return _heap.IndexOf(value) > -1;
        }

        /// <inheritdoc />
        public void Enqueue([NotNull] TVertex value)
        {
            _heap.Add(_distanceFunc(value), value);
        }

        /// <inheritdoc />
        [NotNull]
        public TVertex Dequeue()
        {
            return _heap.RemoveMinimum().Value;
        }

        /// <inheritdoc />
        [NotNull]
        public TVertex Peek()
        {
            return _heap.Minimum().Value;
        }

        /// <inheritdoc />
        public TVertex[] ToArray()
        {
            return _heap.ToValueArray();
        }

        #endregion

        /// <summary>
        /// Converts this queue to an array of vertices associated to their distances.
        /// </summary>
        /// <returns>Array composed of elements.</returns>
        [Pure]
        [NotNull]
        public KeyValuePair<TDistance, TVertex>[] ToArray2()
        {
            return _heap.ToPriorityValueArray();
        }

        /// <summary>
        /// Gets an alternative string representation.
        /// </summary>
        /// <returns>String representation.</returns>
        [Pure]
        [NotNull]
        public string ToString2()
        {
            return _heap.ToString2();
        }

        #region IPriorityQueue

        /// <inheritdoc />
        public void Update([NotNull] TVertex value)
        {
            _heap.Update(_distanceFunc(value), value);
        }

        #endregion
    }
}
