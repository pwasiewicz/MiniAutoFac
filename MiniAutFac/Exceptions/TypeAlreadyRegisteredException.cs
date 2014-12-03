// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeAlreadyRegisteredException.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the TypeAlreadyRegisteredException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Exceptions
{
    using System;

    /// <summary>
    /// Occurs when type is already registered in builder.
    /// </summary>
    public class TypeAlreadyRegisteredException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeAlreadyRegisteredException" /> class.
        /// </summary>
        public TypeAlreadyRegisteredException()
            : base("Type already registered!")
        {
        }
    }
}
