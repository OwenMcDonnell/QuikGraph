using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.TopologicalSort
{
    /// <summary>
    /// Topological sort algorithm (can be performed on an acyclic graph).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class SourceFirstTopologicalSortAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly BinaryQueue<TVertex, int> _heap;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFirstTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public SourceFirstTopologicalSortAlgorithm(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
            _heap = new BinaryQueue<TVertex, int>(vertex => InDegrees[vertex]);
        }

        /// <summary>
        /// Sorted vertices.
        /// </summary>
        [NotNull, ItemNotNull]
        public ICollection<TVertex> SortedVertices { get; private set; } = new List<TVertex>();

        /// <summary>
        /// Vertices in degrees.
        /// </summary>
        [NotNull]
        public IDictionary<TVertex, int> InDegrees { get; } = new Dictionary<TVertex, int>();

        /// <summary>
        /// Fired when a vertex is added to the set of sorted vertices.
        /// </summary>
        public event VertexAction<TVertex> VertexAdded;

        private void OnVertexAdded([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);

            VertexAdded?.Invoke(vertex);
        }

        private void InitializeInDegrees()
        {
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                InDegrees.Add(vertex, 0);
            }

            foreach (TEdge edge in VisitedGraph.Edges)
            {
                if (edge.IsSelfEdge())
                    continue;

                ++InDegrees[edge.Target];
            }

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                _heap.Enqueue(vertex);
            }
        }

        /// <summary>
        /// Runs the topological sort and puts the result in the provided list.
        /// </summary>
        /// <param name="vertices">Set of sorted vertices.</param>
        public void Compute([NotNull, ItemNotNull] IList<TVertex> vertices)
        {
            SortedVertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            SortedVertices.Clear();
            Compute();
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;
            InitializeInDegrees();

            while (_heap.Count != 0)
            {
                if (cancelManager.IsCancelling)
                    break;

                TVertex vertex = _heap.Dequeue();
                if (InDegrees[vertex] != 0)
                    throw new NonAcyclicGraphException();

                SortedVertices.Add(vertex);
                OnVertexAdded(vertex);

                // Update the count of its adjacent vertices
                foreach (TEdge edge in VisitedGraph.OutEdges(vertex))
                {
                    if (edge.IsSelfEdge())
                        continue;

                    --InDegrees[edge.Target];

                    Debug.Assert(InDegrees[edge.Target] >= 0);

                    _heap.Update(edge.Target);
                }
            }
        }

        #endregion
    }
}
