// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamespaceNotRecognizedExcpetion.cs" company="pwasiewicz">
//   pwasiewicz
// </copyright>
// <summary>
//   Defines the NamespaceNotRecognizedExcpetion type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Exceptions
{
    using System;

    /// <summary>The namespace not recognized exception.</summary>
    public class NamespaceNotRecognizedException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="NamespaceNotRecognizedException"/> class.</summary>
        public NamespaceNotRecognizedException()
            : base("Provided namespace is not recognized.")
        {
        }
    }
}
