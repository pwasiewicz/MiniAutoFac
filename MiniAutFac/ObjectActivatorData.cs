// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectActivatorData.cs" company="Wąsiewicz">
//   Patryk Wąsiewicz
// </copyright>
// <summary>
//   Defines the ObjectActivatorData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MiniAutFac.Interfaces;

    /// <summary>
    /// The object activator data.
    /// </summary>
    internal class ObjectActivatorData : IObjectActivatorData
    {
        /// <summary>
        /// Gets or sets the resolved type.
        /// </summary>
        public Type ResolvedType { get; set; }

        /// <summary>
        /// Gets or sets the constructor info.
        /// </summary>
        public ConstructorInfo ConstructorInfo { get; set; }

        /// <summary>
        /// Gets or sets the constructor arguments.
        /// </summary>
        public IEnumerable<object> ConstructorArguments { get; set; }
    }
}
