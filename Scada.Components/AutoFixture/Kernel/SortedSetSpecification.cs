using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a
    /// <see cref="SortedSet{T}"/>.
    /// </summary>
    [Obsolete("This specification is obsolete. Use ExactTypeSpecification(typeof(SortedSet<>)) instead.")]
    public class SortedSetSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for a
        /// <see cref="SortedSet{T}"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a
        /// <see cref="SortedSet{T}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return request is Type type &&
                   type.TryGetSingleGenericTypeArgument(typeof(SortedSet<>), out var _);
        }
    }
}