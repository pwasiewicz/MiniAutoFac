// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IObjectActivatorData.cs" company="Wąsiewicz">
//   
// </copyright>
// <summary>
//   The ObjectActivatorData interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The ObjectActivatorData interface.
    /// </summary>
    public interface IObjectActivatorData
    {
        /// <summary>
        /// Gets or sets the resolved type.
        /// </summary>
        Type ResolvedType { get; }

        /// <summary>
        /// Gets or sets the constructor info.
        /// </summary>
        ConstructorInfo ConstructorInfo { get; }

        /// <summary>
        /// Gets or sets the constructor arguments.
        /// </summary>
        IEnumerable<object> ConstructorArguments { get; }
    }
}
