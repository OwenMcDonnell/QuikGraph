using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Test helpers for graphs.
    /// </summary>
    internal static class GraphTestHelpers
    {
        public static void AssertVertexCountEqual<TVertex>(
            [NotNull] this IVertexSet<TVertex> left,
            [NotNull] IVertexSet<TVertex> right)
        {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.AreEqual(left.VertexCount, right.VertexCount);
        }

        public static void AssertEdgeCountEqual<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> left,
            [NotNull] IEdgeListGraph<TVertex, TEdge> right)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.AreEqual(left.EdgeCount, right.EdgeCount);
        }

        public static bool InVertexSet<TVertex>(
            [NotNull] IVertexSet<TVertex> graph,
            [NotNull] TVertex vertex)
        {
            return graph.ContainsVertex(vertex);
        }

        public static bool InVertexSet<TVertex, TEdge>(
            [NotNull] IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge.Source)
                   && InVertexSet(graph, edge.Target);
        }

        public static bool InEdgeSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge) && graph.ContainsEdge(edge);
        }

        public static void ContainsEdgeAssertions(
            [NotNull] IUndirectedGraph<int, IEdge<int>> graph,
            [NotNull] IEdge<int> e12,
            [NotNull] IEdge<int> f12,
            [CanBeNull] IEdge<int> e21,
            [CanBeNull] IEdge<int> f21)
        {
            Assert.AreEqual(1, graph.AdjacentDegree(1));
            Assert.AreEqual(1, graph.AdjacentDegree(2));
            Assert.AreEqual(1, graph.AdjacentEdges(1).Count());
            Assert.AreEqual(1, graph.AdjacentEdges(2).Count());

            // e12 must be present in u, because we added it.
            Assert.IsTrue(graph.ContainsEdge(e12));

            // f12 is also in u, because e12 == f12.
            Assert.IsTrue(graph.ContainsEdge(f12));

            // e21 and f21 are not in u, because ContainsEdge has semantics that 
            // if it returns true for an edge, that edge must be physically present in 
            // the collection of edges inside u.
            if (e21 != null)
                Assert.IsFalse(graph.ContainsEdge(e21));
            if (f21 != null)
                Assert.IsFalse(graph.ContainsEdge(f21));

            // There must be an edge between vertices 1, 2.
            Assert.IsTrue(graph.ContainsEdge(1, 2));

            // There is also an edge between vertices 2, 1, because the graph is undirected.
            Assert.IsTrue(graph.ContainsEdge(2, 1));

            // Obviously no edge between vertices 1, 3, as vertex 3 is not even present in the graph.
            Assert.IsFalse(graph.ContainsEdge(1, 3));
        }

        public static void BidirectionalContainsEdgeAssertions(
            [NotNull] IBidirectionalGraph<int, IEdge<int>> graph,
            [NotNull] IEdge<int> e12,
            [NotNull] IEdge<int> f12,
            [CanBeNull] IEdge<int> e21,
            [CanBeNull] IEdge<int> f21)
        {
            Assert.AreEqual(0, graph.InDegree(1));
            Assert.AreEqual(1, graph.OutDegree(1));
            Assert.AreEqual(1, graph.InDegree(2));
            Assert.AreEqual(0, graph.OutDegree(2));
            Assert.AreEqual(1, graph.OutEdges(1).Count());
            Assert.AreEqual(1, graph.InEdges(2).Count());

            // e12 must be present in u, because we added it.
            Assert.IsTrue(graph.ContainsEdge(e12));

            // f12 is also in u, because e12 == f12.
            Assert.IsTrue(graph.ContainsEdge(f12));

            // e21 and f21 are not in u, because it's a directed graph.
            if (e21 != null)
                Assert.IsFalse(graph.ContainsEdge(e21));
            if (f21 != null)
                Assert.IsFalse(graph.ContainsEdge(f21));

            // There must be an edge between vertices 1, 2.
            Assert.IsTrue(graph.ContainsEdge(1, 2));

            // No edge between vertices 2, 1, because the graph is directed.
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            // ContainsEdge(1, 3) raises contracts violation in IncidenceGraphContract, because 3 is not in the graph.
            // obviously no edge between vertices 1, 3, as vertex 3 is not even present in the graph.
            // Assert.IsFalse(g.ContainsEdge(1, 3));
        }

        [Pure]
        public static bool IsDescendant<TVertex>(
            [NotNull] Dictionary<TVertex, TVertex> parents,
            [NotNull] TVertex u,
            [NotNull] TVertex v)
        {
            TVertex t;
            TVertex current = u;
            do
            {
                t = current;
                current = parents[t];
                if (current.Equals(v))
                    return true;
            }
            while (!t.Equals(current));

            return false;
        }
    }
}