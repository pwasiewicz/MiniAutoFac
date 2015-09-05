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
    using Parameters;

    /// <summary>
    /// The Resolvable interface - for factory.
    /// </summary>
    public interface IResolvable
    {
        object Resolve(Type type, params Parameter[] parameters);
        object Resolve(Type type);
        object ResolveKeyed(Type type, object key);
    }
}
