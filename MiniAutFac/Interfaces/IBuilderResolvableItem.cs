// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuilderResolvableItem.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the IBuilderResolvableItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Interfaces
{
    /// <summary>
    /// The BuilderResolvableItem interface.
    /// </summary>
    public interface IBuilderResolvableItem
    {
        /// <summary>
        /// Determines the output type of registered type with builder.
        /// </summary>
        /// <typeparam name="T">The output type.</typeparam>
        void As<T>();
    }
}
