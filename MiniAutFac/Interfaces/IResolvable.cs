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
    public interface IResolvable : IDisposable
    {
        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolves the instance of type T.
        /// </summary>
        /// <typeparam name="T">Instance of type to create.</typeparam>
        /// <returns>New instance of type T.</returns>
        T Resolve<T>();
    }
}
