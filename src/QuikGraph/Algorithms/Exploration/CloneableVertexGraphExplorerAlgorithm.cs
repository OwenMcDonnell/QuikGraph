#if SUPPORTS_CLONEABLE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Exploration
{
    /// <summary>
    /// Algorithm that explores a graph starting from a given vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IMutableVertexAndEdgeSet<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : ICloneable, IComparable<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloneableVertexGraphExplorerAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public CloneableVertexGraphExplorerAlgorithm(
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloneableVertexGraphExplorerAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        public CloneableVertexGraphExplorerAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Transitions factories.
        /// </summary>
        [NotNull, ItemNotNull]
        public IList<ITransitionFactory<TVertex, TEdge>> TransitionFactories { get; } =
            new List<ITransitionFactory<TVertex, TEdge>>();

        /// <summary>
        /// Predicate that a vertex must match to be added in the graph.
        /// </summary>
        [NotNull]
        public VertexPredicate<TVertex> AddVertexPredicate { get; set; } = vertex => true;

        /// <summary>
        /// Predicate that checks if a given vertex should be explored or ignored.
        /// </summary>
        [NotNull]
        public VertexPredicate<TVertex> ExploreVertexPredicate { get; set; } = vertex => true;

        /// <summary>
        /// Predicate that an edge must match to be added in the graph.
        /// </summary>
        [NotNull]
        public EdgePredicate<TVertex, TEdge> AddEdgePredicate { get; set; } = edge => true;

        /// <summary>
        /// Predicate that checks if the exploration is finished or not.
        /// </summary>
        [NotNull]
        public Predicate<CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>> FinishedPredicate { get; set; } =
            new DefaultFinishedPredicate().Test;

        [NotNull, ItemNotNull]
        private readonly Queue<TVertex> _unExploredVertices = new Queue<TVertex>();

        /// <summary>
        /// Gets the enumeration of unexplored vertices.
        /// </summary>
        [NotNull, ItemNotNull]
        public IEnumerable<TVertex> UnExploredVertices => _unExploredVertices.AsEnumerable();

        /// <summary>
        /// Indicates if the algorithm finished successfully or not.
        /// </summary>
        public bool FinishedSuccessfully { get; private set; }

        #region Events

        /// <summary>
        /// Fired when a vertex is discovered.
        /// </summary>
        public event VertexAction<TVertex> DiscoverVertex;

        private void OnVertexDiscovered([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);

            VisitedGraph.AddVertex(vertex);
            _unExploredVertices.Enqueue(vertex);

            DiscoverVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge is encountered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when a back edge is encountered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> BackEdge;

        private void OnBackEdge([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge was skipped from exploration due to failed vertex or edge predicate check.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> EdgeSkipped;

        private void OnEdgeSkipped([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            EdgeSkipped?.Invoke(edge);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out TVertex root))
                throw new InvalidOperationException("RootVertex is not specified.");

            VisitedGraph.Clear();
            _unExploredVertices.Clear();
            FinishedSuccessfully = false;

            if (!AddVertexPredicate(root))
                throw new ArgumentException($"StartVertex does not satisfy the {nameof(AddVertexPredicate)}.");
            OnVertexDiscovered(root);

            while (_unExploredVertices.Count > 0)
            {
                // Are we done yet?
                if (!FinishedPredicate(this))
                {
                    FinishedSuccessfully = false;
                    return;
                }

                TVertex current = _unExploredVertices.Dequeue();
                TVertex clone = (TVertex)current.Clone();

                // Let's make sure we want to explore this one
                if (!ExploreVertexPredicate(clone))
                    continue;

                foreach (ITransitionFactory<TVertex, TEdge> transitionFactory in TransitionFactories)
                {
                    GenerateFromTransitionFactory(clone, transitionFactory);
                }
            }

            FinishedSuccessfully = true;
        }

        #endregion

        private void GenerateFromTransitionFactory(
            [NotNull] TVertex current,
            [NotNull] ITransitionFactory<TVertex, TEdge> transitionFactory)
        {
            if (current == null)
                throw new ArgumentNullException(nameof(current));
            if (transitionFactory is null)
                throw new ArgumentNullException(nameof(transitionFactory));

            if (!transitionFactory.IsValid(current))
                return;

            foreach (TEdge transition in transitionFactory.Apply(current))
            {
                if (!AddVertexPredicate(transition.Target)
                    || !AddEdgePredicate(transition))
                {
                    OnEdgeSkipped(transition);
                    continue;
                }

                bool backEdge = VisitedGraph.ContainsVertex(transition.Target);
                if (!backEdge)
                    OnVertexDiscovered(transition.Target);

                VisitedGraph.AddEdge(transition);
                if (backEdge)
                    OnBackEdge(transition);
                else
                    OnTreeEdge(transition);
            }
        }

        /// <summary>
        /// Default implementation of the finished predicate.
        /// </summary>
        public sealed class DefaultFinishedPredicate
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultFinishedPredicate"/> class.
            /// </summary>
            public DefaultFinishedPredicate()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultFinishedPredicate"/> class.
            /// </summary>
            /// <param name="maxVertexCount">Maximum number of vertices.</param>
            /// <param name="maxEdgeCount">Maximum number of edges.</param>
            public DefaultFinishedPredicate(int maxVertexCount, int maxEdgeCount)
            {
                MaxVertexCount = maxVertexCount;
                MaxEdgeCount = maxEdgeCount;
            }

            /// <summary>
            /// Maximum number of vertices (for exploration).
            /// </summary>
            public int MaxVertexCount { get; set; } = 1000;

            /// <summary>
            /// Maximum number of edges (for exploration).
            /// </summary>
            public int MaxEdgeCount { get; set; } = 1000;

            /// <summary>
            /// Checks if the given <paramref name="algorithm"/> explorer has finished or not.
            /// </summary>
            /// <param name="algorithm">Algorithm explorer to check.</param>
            /// <returns>True if the explorer can continue to explore, false otherwise.</returns>
            [Pure]
            public bool Test(CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge> algorithm)
            {
                if (algorithm.VisitedGraph.VertexCount > MaxVertexCount)
                    return false;

                if (algorithm.VisitedGraph.EdgeCount > MaxEdgeCount)
                    return false;

                return true;
            }
        }
    }
}
#endif