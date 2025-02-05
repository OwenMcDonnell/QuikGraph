#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Represents a vertex list graph that is filtered with a vertex predicate.
    /// This means only vertex matching predicates are "accessible".
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class FilteredVertexListGraph<TVertex, TEdge, TGraph> 
        : FilteredIncidenceGraph<TVertex, TEdge, TGraph>
        , IVertexListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexListGraph<TVertex, TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredVertexListGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="baseGraph">Graph in which applying predicates.</param>
        /// <param name="vertexPredicate">Predicate to match vertex that should be taken into account.</param>
        /// <param name="edgePredicate">Predicate to match edge that should be taken into account.</param>
        public FilteredVertexListGraph(
            [NotNull] TGraph baseGraph,
            [NotNull] VertexPredicate<TVertex> vertexPredicate,
            [NotNull] EdgePredicate<TVertex, TEdge> edgePredicate)
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        /// <inheritdoc />
        public bool IsVerticesEmpty => VertexCount == 0;

        /// <inheritdoc />
        public int VertexCount => Vertices.Count();

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => BaseGraph.Vertices.Where(vertex => VertexPredicate(vertex));
    }
}
