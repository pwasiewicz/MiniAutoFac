// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuilderResolvableItem.cs" company="Wasiewcz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the BuilderResolvableItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac
{
    using System;
    using System.Collections.Generic;
    using MiniAutFac.Exceptions;
    using MiniAutFac.Interfaces;
    using MiniAutFac.Parameters;

    /// <summary>
    /// The builder resolvable item.
    /// </summary>
    internal class BuilderResolvableItem : IBuilderResolvableItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderResolvableItem" /> class.
        /// </summary>
        /// <param name="inType">Type of the input.</param>
        internal BuilderResolvableItem(Type inType)
        {
            this.Parameters = new List<Parameter>();

            this.InType = inType;
            this.AsType = this.InType;
        }

        /// <summary>
        /// Gets or sets the registered type.
        /// </summary>
        /// <value> The type of the input.  </value>
        internal Type InType { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        internal List<Parameter> Parameters { get; set; } 

        /// <summary>
        /// Gets or sets the output type.
        /// </summary>
        /// <value>The type of the output. </value>
        internal Type AsType { get; set; }

        /// <summary>
        /// Determines the output type of registered type with builder.
        /// </summary>
        /// <typeparam name="T">The output type.</typeparam>
        public void As<T>()
        {
            var asType = typeof(T);
            if (!asType.IsAssignableFrom(this.InType))
            {
                throw new NotAssignableException();
            }

            this.AsType = asType;
        }
    }
}
