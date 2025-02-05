using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Serialization;

namespace QuikGraph.Tests.Serialization
{
    /// <summary>
    /// Tests for <see cref="GraphMLSerializer{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal class GraphMLSerializerWithArgumentsTests
    {
        #region Test classes

        public sealed class TestGraph : AdjacencyGraph<TestVertex, TestEdge>
        {
            [XmlAttribute("g_string")]
            [DefaultValue("defaultValue")]
            public string String { get; set; }

            [XmlAttribute("g_int")]
            [DefaultValue(1)]
            public int Int { get; set; }

            [XmlAttribute("g_long")]
            [DefaultValue(2L)]
            public long Long { get; set; }

            [XmlAttribute("g_bool")]
            public bool Bool { get; set; }

            [XmlAttribute("g_float")]
            public float Float { get; set; }

            [XmlAttribute("g_double")]
            public double Double { get; set; }

            [XmlAttribute("g_stringArray")]
            public string[] StringArray { get; set; }

            [XmlAttribute("g_intArray")]
            public int[] IntArray { get; set; }

            [XmlAttribute("g_longArray")]
            public long[] LongArray { get; set; }

            [XmlAttribute("g_boolArray")]
            public bool[] BoolArray { get; set; }

            [XmlAttribute("g_floatArray")]
            public float[] FloatArray { get; set; }

            [XmlAttribute("g_doubleArray")]
            public double[] DoubleArray { get; set; }

            [XmlAttribute("g_nullArray")]
            public int[] NullArray
            {
                get => null;
                set
                {
                    Assert.IsNull(value);
                }
            }

            #region IList properties

            [XmlAttribute("g_stringIList")]
            public IList<string> StringIList { get; set; }

            [XmlAttribute("g_intIList")]
            public IList<int> IntIList { get; set; }

            [XmlAttribute("g_longIList")]
            public IList<long> LongIList { get; set; }

            [XmlAttribute("g_boolIList")]
            public IList<bool> BoolIList { get; set; }

            [XmlAttribute("g_floatIList")]
            public IList<float> FloatIList { get; set; }

            [XmlAttribute("g_doubleIList")]
            public IList<double> DoubleIList { get; set; }

            #endregion
        }

        public sealed class TestVertex
        {
            public TestVertex([NotNull] string id)
            {
                ID = id;
            }

            [NotNull]
            public string ID { get; }

            [XmlAttribute("v_stringDefault")]
            [DefaultValue("defaultDefaultString")]
            public string StringDefault { get; set; }

            [XmlAttribute("v_string")]
            [DefaultValue("defaultString")]
            public string String { get; set; }

            [XmlAttribute("v_int")]
            [DefaultValue(1)]
            public int Int { get; set; }

            [XmlAttribute("v_long")]
            [DefaultValue(2L)]
            public long Long { get; set; }

            [XmlAttribute("v_bool")]
            public bool Bool { get; set; }

            [XmlAttribute("v_float")]
            public float Float { get; set; }

            [XmlAttribute("v_double")]
            public double Double { get; set; }

            [XmlAttribute("v_intArray")]
            public int[] IntArray { get; set; }

            [XmlAttribute("v_intIList")]
            public IList<int> IntIList { get; set; }
        }

        public sealed class TestEdge : Edge<TestVertex>
        {
            public TestEdge(
                [NotNull] TestVertex source,
                [NotNull] TestVertex target,
                [NotNull] string id)
                : base(source, target)
            {
                ID = id;
            }

            public TestEdge(
                [NotNull] TestVertex source,
                [NotNull] TestVertex target,
                [NotNull] string id,
                string @string,
                int @int,
                long @long,
                float @float,
                double @double,
                bool @bool)
                : this(source, target, id)
            {
                String = @string;
                Int = @int;
                Long = @long;
                Float = @float;
                Double = @double;
                Bool = @bool;
            }

            [NotNull]
            public string ID { get; }

            [XmlAttribute("e_string")]
            [DefaultValue("defaultString")]
            public string String { get; set; }

            [XmlAttribute("e_int")]
            [DefaultValue(1)]
            public int Int { get; set; }

            [XmlAttribute("e_long")]
            [DefaultValue(2L)]
            public long Long { get; set; }

            [XmlAttribute("e_double")]
            public double Double { get; set; }

            [XmlAttribute("e_bool")]
            public bool Bool { get; set; }

            [XmlAttribute("e_float")]
            public float Float { get; set; }
        }

        #endregion

        #region Test data

        [NotNull]
        private static readonly bool[] Bools = { true, false, true, true };

        [NotNull]
        private static readonly int[] Ints = { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };

        [NotNull]
        private static readonly long[] Longs = { 3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232 };

        [NotNull]
        private static readonly float[] Floats = { 3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F };

        [NotNull]
        private static readonly double[] Doubles = { 3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345 };

        [NotNull, ItemNotNull]
        private static readonly string[] Strings = { "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", "" };


        [NotNull]
        private static readonly IList<bool> BoolsList = new[] { true, false, true, true };

        [NotNull]
        private static readonly IList<int> IntsList = new[] { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };

        [NotNull]
        private static readonly IList<long> LongsList = new[] { 3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232 };

        [NotNull]
        private static readonly IList<float> FloatsList = new[] { 3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F };

        [NotNull]
        private static readonly IList<double> DoublesList = new[] { 3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345 };

        [NotNull, ItemNotNull]
        private static readonly IList<string> StringsList = new[] { "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", "" };

        #endregion

        #region Helpers

        [Pure]
        [NotNull]
        private static TestGraph VerifySerialization([NotNull] TestGraph graph)
        {
            string xml;
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
                        xmlWriter,
                        vertex => vertex.ID,
                        edge => edge.ID);
                }

                xml = writer.ToString();
            }

            TestGraph serializedGraph;
            using (var reader = new StringReader(xml))
            {
                serializedGraph = new TestGraph();
                serializedGraph.DeserializeAndValidateFromGraphML(
                    reader,
                    id => new TestVertex(id),
                    (source, target, id) => new TestEdge(source, target, id));
            }

            string newXml;
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    serializedGraph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
                        xmlWriter,
                        vertex => vertex.ID,
                        edge => edge.ID);
                }

                newXml = writer.ToString();
            }

            Assert.AreEqual(xml, newXml);

            return serializedGraph;
        }

        #endregion

        [Test]
        public void WriteVertex()
        {
            var graph = new TestGraph
            {
                Bool = true,
                Double = 1.0,
                Float = 2.0F,
                Int = 10,
                Long = 100,
                String = "foo",
                BoolArray = Bools,
                IntArray = Ints,
                LongArray = Longs,
                FloatArray = Floats,
                DoubleArray = Doubles,
                StringArray = Strings,
                NullArray = null,
                BoolIList = BoolsList,
                IntIList = IntsList,
                LongIList = LongsList,
                FloatIList = FloatsList,
                DoubleIList = DoublesList,
                StringIList = StringsList
            };

            var vertex = new TestVertex("v1")
            {
                StringDefault = "bar",
                String = "string",
                Int = 10,
                Long = 20,
                Float = 25.0F,
                Double = 30.0,
                Bool = true,
                IntArray = new[] { 1, 2, 3, 4 },
                IntIList = new[] { 4, 5, 6, 7 }
            };

            graph.AddVertex(vertex);

            TestGraph serializedGraph = VerifySerialization(graph);
            Assert.AreEqual(graph.Bool, serializedGraph.Bool);
            Assert.AreEqual(graph.Double, serializedGraph.Double);
            Assert.AreEqual(graph.Float, serializedGraph.Float);
            Assert.AreEqual(graph.Int, serializedGraph.Int);
            Assert.AreEqual(graph.Long, serializedGraph.Long);
            Assert.AreEqual(graph.String, serializedGraph.String);
            CollectionAssert.AreEqual(graph.BoolArray, serializedGraph.BoolArray);
            CollectionAssert.AreEqual(graph.IntArray, serializedGraph.IntArray);
            CollectionAssert.AreEqual(graph.LongArray, serializedGraph.LongArray);
            CollectionAssert.AreEqual(graph.StringArray, serializedGraph.StringArray);
            CollectionAssert.AreEqual(graph.FloatArray, serializedGraph.FloatArray, new FloatComparer(0.001F));
            CollectionAssert.AreEqual(graph.DoubleArray, serializedGraph.DoubleArray, new DoubleComparer(0.0001));
            CollectionAssert.AreEqual(graph.BoolIList, serializedGraph.BoolIList);
            CollectionAssert.AreEqual(graph.IntIList, serializedGraph.IntIList);
            CollectionAssert.AreEqual(graph.LongIList, serializedGraph.LongIList);
            CollectionAssert.AreEqual(graph.StringIList, serializedGraph.StringIList);
            CollectionAssert.AreEqual(graph.FloatIList, serializedGraph.FloatIList, new FloatComparer(0.001F));
            CollectionAssert.AreEqual(graph.DoubleIList, serializedGraph.DoubleIList, new DoubleComparer(0.0001));

            TestVertex serializedVertex = serializedGraph.Vertices.First();
            Assert.AreEqual("bar", serializedVertex.StringDefault);
            Assert.AreEqual(vertex.String, serializedVertex.String);
            Assert.AreEqual(vertex.Int, serializedVertex.Int);
            Assert.AreEqual(vertex.Long, serializedVertex.Long);
            Assert.AreEqual(vertex.Float, serializedVertex.Float);
            Assert.AreEqual(vertex.Double, serializedVertex.Double);
            Assert.AreEqual(vertex.Bool, serializedVertex.Bool);
            CollectionAssert.AreEqual(vertex.IntArray, serializedVertex.IntArray);
            CollectionAssert.AreEqual(vertex.IntIList, serializedVertex.IntIList);
        }

        [Test]
        public void WriteEdge()
        {

            var graph = new TestGraph
            {
                Bool = true,
                Double = 1.0,
                Float = 2.0F,
                Int = 10,
                Long = 100,
                String = "foo"
            };

            var vertex1 = new TestVertex("v1")
            {
                StringDefault = "foo",
                String = "string",
                Int = 10,
                Long = 20,
                Float = 25.0F,
                Double = 30.0,
                Bool = true
            };

            var vertex2 = new TestVertex("v2")
            {
                StringDefault = "bar",
                String = "string2",
                Int = 110,
                Long = 120,
                Float = 125.0F,
                Double = 130.0,
                Bool = true
            };

            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);

            var edge = new TestEdge(
                vertex1, vertex2,
                "e1",
                "edge",
                90,
                100,
                25.0F,
                110.0,
                true);

            graph.AddEdge(edge);

            TestGraph serializedGraph = VerifySerialization(graph);

            TestEdge serializedEdge = serializedGraph.Edges.First();
            Assert.AreEqual(edge.ID, serializedEdge.ID);
            Assert.AreEqual(edge.String, serializedEdge.String);
            Assert.AreEqual(edge.Int, serializedEdge.Int);
            Assert.AreEqual(edge.Long, serializedEdge.Long);
            Assert.AreEqual(edge.Float, serializedEdge.Float);
            Assert.AreEqual(edge.Double, serializedEdge.Double);
            Assert.AreEqual(edge.Bool, serializedEdge.Bool);
        }
    }
}
