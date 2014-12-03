// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepositoryEmptyException.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the TypeRepositoryEmptyException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Exceptions
{
    using System;

    /// <summary>
    /// Occurs when repository is null.
    /// </summary>
    public class TypeRepositoryEmptyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepositoryEmptyException"/> class.
        /// </summary>
        public TypeRepositoryEmptyException()
            : base("Repository of types is empty!")
        {
        }
    }
}
