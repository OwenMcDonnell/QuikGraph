using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Predicate that tests if an edge's reverse is residual.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class ReversedResidualEdgePredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReversedResidualEdgePredicate{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="residualCapacities">Residual capacities per edge.</param>
        /// <param name="reversedEdges">Map of edges and their reversed edges.</param>
        public ReversedResidualEdgePredicate(
            [NotNull] IDictionary<TEdge, double> residualCapacities,
            [NotNull] IDictionary<TEdge, TEdge> reversedEdges)
        {
            if (residualCapacities is null)
                throw new ArgumentNullException(nameof(residualCapacities));
            if (reversedEdges is null)
                throw new ArgumentNullException(nameof(reversedEdges));
            ResidualCapacities = residualCapacities;
            ReversedEdges = reversedEdges;
        }

        /// <summary>
        /// Residual capacities map.
        /// </summary>
        [NotNull]
        public IDictionary<TEdge, double> ResidualCapacities { get; }

        /// <summary>
        /// Reversed edges map.
        /// </summary>
        [NotNull]
        public IDictionary<TEdge, TEdge> ReversedEdges { get; }

        /// <summary>
        /// Checks if the given <paramref name="edge"/> reverse is residual.
        /// </summary>
        /// <remarks>Check if the implemented predicate is matched.</remarks>
        /// <param name="edge">Edge to check.</param>
        /// <returns>True if the reversed edge is residual, false otherwise.</returns>
        [Pure]
        public bool Test([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));
            return 0 < ResidualCapacities[ReversedEdges[edge]];
        }
    }
}
