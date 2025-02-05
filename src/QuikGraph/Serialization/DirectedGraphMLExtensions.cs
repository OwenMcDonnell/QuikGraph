using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;
using QuikGraph.Algorithms;
using QuikGraph.Serialization.DirectedGraphML;

namespace QuikGraph.Serialization
{
    /// <summary>
    /// Directed graph Markup Language extensions.
    /// </summary>
    public static class DirectedGraphMLExtensions
    {
        [CanBeNull]
        private static XmlSerializer _directedGraphSerializer;

        /// <summary>
        /// Gets the DirectedGraph XML serializer.
        /// </summary>
        public static XmlSerializer DirectedGraphSerializer =>
            _directedGraphSerializer ?? (_directedGraphSerializer = new XmlSerializer(typeof(DirectedGraph)));

        /// <summary>
        /// Writes the DGML data structure to the XML writer.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="filePath">Path to the file to write into.</param>
        public static void WriteXml([NotNull] this DirectedGraph graph, [NotNull] string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Must provide a file path.", nameof(filePath));

            using (StreamWriter stream = File.CreateText(filePath))
                WriteXml(graph, stream);
        }

        /// <summary>
        /// Writes the DGML data structure to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="writer">XML writer in which writing graph data.</param>
        public static void WriteXml([NotNull] this DirectedGraph graph, [NotNull] XmlWriter writer)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            DirectedGraphSerializer.Serialize(writer, graph);
        }

        /// <summary>
        /// Writes the DGML data structure to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="stream">Stream in which writing graph data.</param>
        public static void WriteXml([NotNull] this DirectedGraph graph, [NotNull] Stream stream)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            DirectedGraphSerializer.Serialize(stream, graph);
        }

        /// <summary>
        /// Writes the DGML data structure to the <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="writer">Text writer in which writing graph data.</param>
        public static void WriteXml([NotNull] this DirectedGraph graph, [NotNull] TextWriter writer)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            DirectedGraphSerializer.Serialize(writer, graph);
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="visitedGraph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <returns>Converted graph.</returns>
        [NotNull]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>([NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
            return ToDirectedGraphML(
                visitedGraph,
                visitedGraph.GetVertexIdentity(),
                visitedGraph.GetEdgeIdentity());
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="visitedGraph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <param name="vertexColors">Function that gives the color of a vertex.</param>
        /// <returns>Converted graph.</returns>
        [NotNull]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TVertex, GraphColor> vertexColors)
            where TEdge : IEdge<TVertex>
        {
            return ToDirectedGraphML(
                visitedGraph,
                visitedGraph.GetVertexIdentity(),
                visitedGraph.GetEdgeIdentity(),
                (vertex, node) =>
                {
                    GraphColor color = vertexColors(vertex);
                    switch (color)
                    {
                        case GraphColor.Black:
                            node.Background = "Black";
                            break;
                        case GraphColor.Gray:
                            node.Background = "LightGray";
                            break;
                        case GraphColor.White:
                            node.Background = "White";
                            break;
                    }
                },
                null);
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="visitedGraph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <param name="vertexIdentities">Vertex identity method.</param>
        /// <param name="edgeIdentities">Edge identity method.</param>
        /// <returns>Converted graph.</returns>
        [NotNull]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] VertexIdentity<TVertex> vertexIdentities,
            [NotNull] EdgeIdentity<TVertex, TEdge> edgeIdentities)
            where TEdge : IEdge<TVertex>
        {
            return ToDirectedGraphML(
                visitedGraph,
                vertexIdentities,
                edgeIdentities,
                null,
                null);
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="visitedGraph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <param name="vertexIdentities">Vertex identity method.</param>
        /// <param name="edgeIdentities">Edge identity method.</param>
        /// <param name="formatNode">Formats a vertex into a <see cref="DirectedGraphNode"/>.</param>
        /// <param name="formatEdge">Formats an edge into a <see cref="DirectedGraphLink"/>.</param>
        /// <returns>Converted graph.</returns>
        [NotNull]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] VertexIdentity<TVertex> vertexIdentities,
            [NotNull] EdgeIdentity<TVertex, TEdge> edgeIdentities,
            [CanBeNull] Action<TVertex, DirectedGraphNode> formatNode,
            [CanBeNull] Action<TEdge, DirectedGraphLink> formatEdge)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new DirectedGraphMLAlgorithm<TVertex, TEdge>(
                visitedGraph,
                vertexIdentities,
                edgeIdentities);

            if (formatNode != null)
                algorithm.FormatNode += formatNode;
            if (formatEdge != null)
                algorithm.FormatEdge += formatEdge;
            algorithm.Compute();

            return algorithm.DirectedGraph;
        }

        /// <summary>
        /// Saves and opens the given <paramref name="graph"/> as DGML.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to open.</param>
        /// <param name="filePath">Path to the file to save.</param>
        public static void OpenAsDGML<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [CanBeNull] string filePath)
            where TEdge : IEdge<TVertex>
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            if (filePath is null)
                filePath = "graph.DGML";

            graph.ToDirectedGraphML().WriteXml(filePath);

            if (Debugger.IsAttached)
            {
                Process.Start(filePath);
            }
        }
    }
}
