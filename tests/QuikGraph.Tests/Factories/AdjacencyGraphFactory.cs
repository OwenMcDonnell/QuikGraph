using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Factory to create <see cref="AdjacencyGraph{TVertex,TEdge}"/> for tests.
    /// </summary>
    internal static class AdjacencyGraphFactory
    {
        /// <summary>
        /// Creates an <see cref="AdjacencyGraph{TVertex,TEdge}"/> with given <paramref name="edges"/>.
        /// </summary>
        [Pure]
        [NotNull]
        public static AdjacencyGraph<int, Edge<int>> Create(
            bool allowParallelEdges,
            [NotNull] KeyValuePair<int, int>[] edges)
        {
            Assert.IsNotNull(edges);

            var adjacencyGraph = new AdjacencyGraph<int, Edge<int>>(allowParallelEdges);
            if (edges.Length <= 3)
            {
                foreach (KeyValuePair<int, int> edge in edges)
                    adjacencyGraph.AddVerticesAndEdge(new Edge<int>(edge.Key, edge.Value));
            }

            return adjacencyGraph;
        }
    }
}
