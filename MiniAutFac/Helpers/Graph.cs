// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Graph.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the Graph type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The direct graph representation.
    /// </summary>
    /// <typeparam name="T">The type of vertexes.</typeparam>
    public class Graph<T>
    {
        /// <summary>
        /// The adjacency list.
        /// </summary>
        private readonly IDictionary<T, IList<T>> adjacencyList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph{T}"/> class.
        /// </summary>
        public Graph()
        {
            this.adjacencyList = new Dictionary<T, IList<T>>();
        }

        /// <summary>
        /// The add edge.
        /// </summary>
        /// <param name="v1">The first vertex</param>
        /// <param name="v2">The second vertex</param>
        public void AddEdge(T v1, T v2)
        {
            if (!this.adjacencyList.ContainsKey(v2))
            {
                this.adjacencyList.Add(v2, new List<T>());
            }

            if (this.adjacencyList.ContainsKey(v1))
            {
                this.adjacencyList[v1].Add(v2);
                return;
            }

            this.adjacencyList.Add(v1, new List<T> { v2 });
        }

        /// <summary>
        /// The has cycle.
        /// </summary>
        /// <returns> True, if graph contains cycle.</returns>
        public bool HasCycle()
        {
            var visited = new Dictionary<T, bool>();
            var buffer = new Dictionary<T, bool>();
            foreach (var key in this.adjacencyList.Keys)
            {
                visited.Add(key, false);
                buffer.Add(key, false);
            }

            return this.adjacencyList.Keys.Any(key => this.HasCycle(key, visited, buffer));
        }

        /// <summary>
        /// The has cycle.
        /// </summary>
        /// <param name="v"> The v. </param>
        /// <param name="visited"> The visited.  </param>
        /// <param name="buffer"> The buffer. </param>
        /// <returns>  The <see cref="bool"/>. </returns>
        private bool HasCycle(T v, IDictionary<T, bool> visited, IDictionary<T, bool> buffer)
        {
            if (!visited[v])
            {
                visited[v] = true;
                buffer[v] = true;

                foreach (var neighbourhood in this.adjacencyList[v])
                {
                    if (!visited[neighbourhood] && this.HasCycle(neighbourhood, visited, buffer))
                    {
                        return true;
                    }

                    if (buffer[neighbourhood])
                    {
                        return true;
                    }
                }
            }

            buffer[v] = false;
            return false;
        }
    }
}
