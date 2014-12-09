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
    using System.Linq;
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
        internal BuilderResolvableItem(ContainerBuilder builder, params Type[] inTypes)
            : base(builder)
        {
            this.Parameters = new List<Parameter>();

            this.InTypes = inTypes;

            if (this.InTypes.Length == 1)
            {
                this.AsType = this.InTypes.Single();
            }
        }

        /// <summary>
        /// Determines the output type of registered type with builder.
        /// </summary>
        public override void As(Type type)
        {
            if (this.InTypes.Any(inType => !type.IsAssignableFrom(inType)))
            {
                throw new NotAssignableException();
            }

            this.AsType = type;
        }
    }
}
