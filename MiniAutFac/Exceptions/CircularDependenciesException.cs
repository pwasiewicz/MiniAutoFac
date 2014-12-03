// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CirrcularDependenciesException.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the CirrcularDependenciesException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Exceptions
{
    using System;

    /// <summary>
    /// Occurs when circular dependencies detected.
    /// </summary>
    public class CircularDependenciesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDependenciesException" /> class.
        /// </summary>
        public CircularDependenciesException()
            : base("Cirrcular dependencies detected.")
        {
        }
    }
}
