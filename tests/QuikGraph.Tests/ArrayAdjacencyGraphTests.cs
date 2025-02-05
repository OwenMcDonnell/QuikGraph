using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests for <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/>s.
    /// </summary>
    [TestFixture]
    internal class ArrayAdjacencyGraphTests
    {
        #region Helpers

        private static void SameVertexCount<TVertex, TEdge>([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            ArrayAdjacencyGraph<TVertex, TEdge> adjacencyGraph = graph.ToArrayAdjacencyGraph();
            Assert.AreEqual(graph.VertexCount, adjacencyGraph.VertexCount);
        }

        private static void SameVertices<TVertex, TEdge>([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            ArrayAdjacencyGraph<TVertex, TEdge> adjacencyGraph = graph.ToArrayAdjacencyGraph();
            CollectionAssert.AreEqual(graph.Vertices, adjacencyGraph.Vertices);
        }

        private static void SameEdgeCount<TVertex, TEdge>([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            ArrayAdjacencyGraph<TVertex, TEdge> adjacencyGraph = graph.ToArrayAdjacencyGraph();
            Assert.AreEqual(graph.EdgeCount, adjacencyGraph.EdgeCount);
        }

        private static void SameEdges<TVertex, TEdge>([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            ArrayAdjacencyGraph<TVertex, TEdge> adjacencyGraph = graph.ToArrayAdjacencyGraph();
            CollectionAssert.AreEqual(graph.Edges, adjacencyGraph.Edges);
        }

        private static void SameOutEdges<TVertex, TEdge>([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var adjacencyGraph = graph.ToArrayAdjacencyGraph();
            foreach (TVertex vertex in graph.Vertices)
                CollectionAssert.AreEqual(graph.OutEdges(vertex), adjacencyGraph.OutEdges(vertex));
        }

        #endregion

        [Test]
        public void SameVertexCountAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                SameVertexCount(graph);
        }

        [Test]
        public void SameVerticesAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                SameVertices(graph);
        }

        [Test]
        public void SameEdgeCountAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                SameEdgeCount(graph);
        }

        [Test]
        public void SameEdgesAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                SameEdges(graph);
        }

        [Test]
        public void SameOutEdgesAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                SameOutEdges(graph);
        }
    }
}
