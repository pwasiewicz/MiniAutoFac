// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResolvable.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the IResolvable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Interfaces
{
    using System;

    /// <summary>
    /// The Resolvable interface - for factory.
    /// </summary>
    public interface IResolvable
    {

        object Resolve(Type type);
    }
}
