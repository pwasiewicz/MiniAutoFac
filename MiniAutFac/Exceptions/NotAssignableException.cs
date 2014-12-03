// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotAssignableException.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   The not assignable excpetion.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Exceptions
{
    using System;

    /// <summary>
    /// Occurs when output type is not assignable from registered types.
    /// </summary>
    public class NotAssignableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotAssignableException" /> class.
        /// </summary>
        public NotAssignableException()
            : base("Ouptut type is not assignable from source type!")
        {
        }
    }
}
