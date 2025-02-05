using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Implementation for an immutable bidirectional directed graph data structure for large sparse
    /// graph representation where out-edge need to be enumerated only.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public sealed class ArrayBidirectionalGraph<TVertex, TEdge> : IBidirectionalGraph<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
#if SUPPORTS_SERIALIZATION
        [Serializable]
#endif
        private struct InOutEdges
        {
            [NotNull]
            private readonly TEdge[] _outEdges;

            [NotNull]
            private readonly TEdge[] _inEdges;

            public InOutEdges([NotNull] TEdge[] outEdges, [NotNull] TEdge[] inEdges)
            {
                _outEdges = outEdges;
                _inEdges = inEdges;
            }

            public bool TryGetOutEdges([NotNull] out TEdge[] edges)
            {
                edges = _outEdges;
                return _outEdges.Length > 0;
            }

            public bool TryGetInEdges([NotNull] out TEdge[] edges)
            {
                edges = _inEdges;
                return _inEdges.Length > 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public ArrayBidirectionalGraph([NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
        {
            if (visitedGraph is null)
                throw new ArgumentNullException(nameof(visitedGraph));

            _vertexEdges = new Dictionary<TVertex, InOutEdges>(visitedGraph.VertexCount);
            EdgeCount = visitedGraph.EdgeCount;
            foreach (var vertex in visitedGraph.Vertices)
            {
                var outEdges = visitedGraph.OutEdges(vertex).ToArray();
                var inEdges = visitedGraph.InEdges(vertex).ToArray();
                _vertexEdges.Add(vertex, new InOutEdges(outEdges, inEdges));
            }
        }

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges => true;

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _vertexEdges.Count == 0;

        /// <inheritdoc />
        public int VertexCount => _vertexEdges.Count;

        [NotNull]
        private readonly Dictionary<TVertex, InOutEdges> _vertexEdges;

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => _vertexEdges.Keys;

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexEdges.ContainsKey(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount { get; }

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges =>
            _vertexEdges.Values.SelectMany(inOutEdges =>
            {
                if (inOutEdges.TryGetOutEdges(out TEdge[] outEdges))
                    return outEdges;
                return Enumerable.Empty<TEdge>();
            });

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (_vertexEdges.TryGetValue(edge.Source, out InOutEdges inOutEdges) 
                && inOutEdges.TryGetOutEdges(out TEdge[] edges))
            {
                return edges.Any(e => e.Equals(edge));
            }

            return false;
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (_vertexEdges.TryGetValue(source, out InOutEdges inOutEdges) 
                && inOutEdges.TryGetOutEdges(out TEdge[] outEdges))
            {
                foreach (TEdge outEdge in outEdges)
                {
                    if (outEdge.Target.Equals(target))
                    {
                        edge = outEdge;
                        return true;
                    }
                }
            }

            edge = default(TEdge);
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (_vertexEdges.TryGetValue(source, out InOutEdges inOutEdges)
                && inOutEdges.TryGetOutEdges(out TEdge[] outEdges))
            {
                edges = outEdges.Where(edge => edge.Target.Equals(target));
                return edges.Any();
            }

            edges = null;
            return false;
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return OutDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out InOutEdges inOutEdges) 
                && inOutEdges.TryGetOutEdges(out TEdge[] outEdges))
                return outEdges.Length;
            return 0;
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (TryGetOutEdges(vertex, out IEnumerable<TEdge> outEdges))
                return outEdges;
            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out InOutEdges inOutEdges) &&
                inOutEdges.TryGetOutEdges(out TEdge[] outEdges))
            {
                edges = outEdges;
                return true;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!_vertexEdges[vertex].TryGetOutEdges(out TEdge[] edges))
                throw new InvalidOperationException();

            return edges[index];
        }
        
        #endregion

        #region IBidirectionalGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsInEdgesEmpty(TVertex vertex)
        {
            return InDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public int InDegree(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out InOutEdges inOutEdges) 
                && inOutEdges.TryGetInEdges(out TEdge[] inEdges))
                return inEdges.Length;
            return 0;
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            if (TryGetInEdges(vertex, out IEnumerable<TEdge> inEdges))
                return inEdges;
            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out InOutEdges inOutEdges) 
                && inOutEdges.TryGetInEdges(out TEdge[] inEdges))
            {
                edges = inEdges;
                return true;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge InEdge(TVertex vertex, int index)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!_vertexEdges[vertex].TryGetInEdges(out TEdge[] edges))
                throw new InvalidOperationException();

            return edges[index];
        }

        /// <inheritdoc />
        public int Degree(TVertex vertex)
        {
            return InDegree(vertex) + OutDegree(vertex);
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Clones this graph, returns this instance because this class is immutable.
        /// </summary>
        /// <returns>This graph.</returns>
        [Pure]
        [NotNull]
        public ArrayBidirectionalGraph<TVertex, TEdge> Clone()
        {
            return this;
        }

#if SUPPORTS_CLONEABLE
        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return Clone();
        }
#endif

        #endregion
    }
}
