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
        Type ResolvedType { get; set; }

        /// <summary>
        /// Gets or sets the constructor info.
        /// </summary>
        ConstructorInfo ConstructorInfo { get; set; }

        /// <summary>
        /// Gets or sets the constructor arguments.
        /// </summary>
        IEnumerable<object> ConstructorArguments { get; set; } 
    }
}
