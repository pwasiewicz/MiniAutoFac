// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CannotResolveTypeException.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the CannotResolveTypeException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Exceptions
{
    using System;

    /// <summary>
    /// Occurs when cannot resolve type.
    /// </summary>
    public class CannotResolveTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CannotResolveTypeException" /> class.
        /// </summary>
        public CannotResolveTypeException()
            : base("Cannot resolve desired type!")
        {
        }
    }
}
