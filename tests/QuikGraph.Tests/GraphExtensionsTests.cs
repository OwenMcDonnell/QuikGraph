using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class GraphExtensionsTests
    {
        [Test]
        public void DictionaryToVertexAndEdgeListGraph()
        {
            var dictionary = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2 },
                [1] = new[] { 2 },
                [2] = new int[] { }
            };

            var graph = dictionary.ToVertexAndEdgeListGraph(
                kv => Array.ConvertAll(kv.Value, vertex => new SEquatableEdge<int>(kv.Key, vertex)));

            foreach (KeyValuePair<int, int[]> kv in dictionary)
            {
                Assert.IsTrue(graph.ContainsVertex(kv.Key));
                foreach (int i in kv.Value)
                {
                    Assert.IsTrue(graph.ContainsVertex(i));
                    Assert.IsTrue(graph.ContainsEdge(kv.Key, i));
                }
            }
        }

        [Test]
        public void CollectionOfEdgesToAdjacencyGraph()
        {
            int numVertices = 4;
            var vertices = new Collection<TestVertex>();
            for (int i = 0; i < numVertices; ++i)
            {
                vertices.Add(new TestVertex(i.ToString()));
            }

            List<Edge<TestVertex>> edges = TestHelpers.CreateAllPairwiseEdges(
                vertices,
                vertices,
                (source, target) => new Edge<TestVertex>(source, target));

            AdjacencyGraph<TestVertex, Edge<TestVertex>> graph = edges.ToAdjacencyGraph<TestVertex, Edge<TestVertex>>();

            // Verify that the right number of vertices were created
            Assert.AreEqual(numVertices, graph.Vertices.ToList().Count);
        }
    }
}
