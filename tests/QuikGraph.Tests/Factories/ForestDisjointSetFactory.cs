using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Factory to create <see cref="ForestDisjointSet{T}"/> for tests.
    /// </summary>
    internal static class ForestDisjointSetFactory
    {
        /// <summary>
        /// Creates a <see cref="ForestDisjointSet{T}"/> with given <paramref name="elements"/>
        /// unioned with given <paramref name="unions"/>.
        /// </summary>
        [Pure]
        [NotNull]
        public static ForestDisjointSet<int> Create([NotNull] int[] elements, [NotNull] int[] unions)
        {
            var sets = new ForestDisjointSet<int>();
            for (int i = 0; i < elements.Length; ++i)
            {
                sets.MakeSet(i);
            }

            for (int i = 0; i + 1 < unions.Length; i += 2)
            {
                Assert.IsTrue(sets.Contains(unions[i]));
                Assert.IsTrue(sets.Contains(unions[i + 1]));
                sets.Union(unions[i], unions[i + 1]);
            }

            return sets;
        }
    }
}
