using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace QuikGraph.Serialization
{
    /// <summary>
    /// A base class that creates a proxy to a graph that is serializable in XML.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    [XmlRoot("graph")]
    public class XmlSerializableGraph<TVertex, TEdge, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
    {
        private XmlEdgeList _edges;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        public XmlSerializableGraph()
            : this(new TGraph())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="graph">Graph to serialize.</param>
        public XmlSerializableGraph([NotNull] TGraph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            Graph = graph;
        }

        /// <summary>
        /// Gets the graph to serialize.
        /// </summary>
        [NotNull]
        [XmlElement("graph-traits")]
        public TGraph Graph { get; }

        /// <summary>
        /// Gets the edges to serialize.
        /// </summary>
        [NotNull, ItemNotNull]
        [XmlArray("edges")]
        [XmlArrayItem("edge")]
        public IEnumerable<TEdge> Edges => _edges ?? (_edges = new XmlEdgeList(Graph));

        /// <summary>
        /// Represents an XML serializable list of edge.
        /// </summary>
        public class XmlEdgeList : IEnumerable<TEdge>
        {
            [NotNull]
            private readonly TGraph _graph;

            internal XmlEdgeList([NotNull] TGraph graph)
            {
                if (graph == null)
                    throw new ArgumentNullException(nameof(graph));

                _graph = graph;
            }

            #region IEnumerable

            /// <inheritdoc />
            public IEnumerator<TEdge> GetEnumerator()
            {
                return _graph.Edges.GetEnumerator();
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            /// <summary>
            /// Adds an edge to this serializable graph.
            /// </summary>
            /// <param name="edge">Edge to add.</param>
            public void Add([NotNull] TEdge edge)
            {
                if (edge == null)
                    throw new ArgumentNullException(nameof(edge));

                _graph.AddVerticesAndEdge(edge);
            }
        }
    }
}
