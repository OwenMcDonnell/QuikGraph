using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SUndirectedEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal class SUndirectedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SUndirectedEdge<int>(1, 2), 1, 2);
            CheckEdge(new SUndirectedEdge<int>(2, 1), 2, 1);
            CheckEdge(new SUndirectedEdge<int>(1, 1), 1, 1);
            CheckEdge(default(SUndirectedEdge<int>), 0, 0);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SUndirectedEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new SUndirectedEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new SUndirectedEdge<TestVertex>(v1, v1), v1, v1);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(SUndirectedEdge<TestVertex>);
            Assert.IsNull(defaultEdge.Source);
            // ReSharper disable once HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            Assert.IsNull(defaultEdge.Target);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SUndirectedEdge<TestVertex>(null, new TestVertex("v1")));
            Assert.Throws<ArgumentNullException>(() => new SUndirectedEdge<TestVertex>(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new SUndirectedEdge<TestVertex>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = default(SUndirectedEdge<int>);
            var edge2 = new SUndirectedEdge<int>(0, 0);
            var edge3 = new SUndirectedEdge<int>(1, 2);
            var edge4 = new SUndirectedEdge<int>(1, 2);
            var edge5 = new SUndirectedEdge<int>(2, 1);

            Assert.AreEqual(edge1, edge1);
            Assert.AreEqual(edge1, edge2);  // Is equatable
            Assert.AreNotEqual(edge1, edge3);

            Assert.AreEqual(edge3, edge4);
            Assert.AreNotEqual(edge3, edge5);

            Assert.AreNotEqual(edge1, null);
        }
    }
}
