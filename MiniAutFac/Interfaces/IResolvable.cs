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
    /// <summary>
    /// The Resolvable interface - for factory.
    /// </summary>
    public interface IResolvable
    {
        /// <summary>
        /// Resolves the instance of type T.
        /// </summary>
        /// <typeparam name="T">Instance of type to create.</typeparam>
        /// <returns>New instance of type T.</returns>
        T Resolve<T>();
    }
}
