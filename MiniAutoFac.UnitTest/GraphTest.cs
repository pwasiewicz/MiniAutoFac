// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphTest.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   The graph test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutoFac.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MiniAutFac.Helpers;

    /// <summary>
    /// The graph test.
    /// </summary>
    [TestClass]
    public class GraphTest
    {
        /// <summary>
        /// The has cycle test.
        /// </summary>
        [TestMethod]
        public void HasCycle()
        {
            var graph = new Graph<int>();

            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 3);

            Assert.IsTrue(graph.HasCycle());
        }

        /// <summary>
        /// The has cacle 1 vertex.
        /// </summary>
        [TestMethod]
        public void HasCacle1Vertex()
        {
            var graph = new Graph<int>();

            graph.AddEdge(0, 0);

            Assert.IsTrue(graph.HasCycle());
        }

        /// <summary>
        /// The has no cycle.
        /// </summary>
        [TestMethod]
        public void HasNoCycle()
        {
            var graph = new Graph<int>();

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 3);
            graph.AddEdge(1, 2);
            graph.AddEdge(3, 2);

            Assert.IsFalse(graph.HasCycle());
        }

    }
}
