// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuilderResolvableItem.cs" company="Wasiewcz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the BuilderResolvableItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Resolvable
{
    using MiniAutFac.Exceptions;
    using MiniAutFac.Parameters;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The builder resolvable item.
    /// </summary>
    internal sealed class BuilderResolvableItem : BuilderResolvableItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderResolvableItem" /> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="inType">Type of the input.</param>
        internal BuilderResolvableItem(ContainerBuilder builder, Type inType) : base(builder)
        {
            this.Parameters = new List<Parameter>();

            this.InType = inType;
            this.AsType = this.InType;
        }

        /// <summary>
        /// Determines the output type of registered type with builder.
        /// </summary>
        /// <typeparam name="T">The output type.</typeparam>
        public override void As(Type type)
        {
            if (!type.IsAssignableFrom(this.InType))
            {
                throw new NotAssignableException();
            }

            this.AsType = type;
        }
    }
}
