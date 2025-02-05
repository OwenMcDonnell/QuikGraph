using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Factory to create <see cref="Edge{Int32}"/> for tests.
    /// </summary>
    internal static class EdgeFactory
    {
        /// <summary>
        /// Creates an <see cref="Edge{Int32}"/> with given
        /// <paramref name="source"/> and <paramref name="target"/>.
        /// </summary>
        [Pure]
        [NotNull]
        public static Edge<int> Create(int source, int target)
        {
            return new Edge<int>(source, target);
        }
    }
}
