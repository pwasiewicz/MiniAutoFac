// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemRegistration.cs" company="Wasiewcz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the ItemRegistration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Resolvable
{
    using MiniAutFac.Exceptions;
    using MiniAutFac.Parameters;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The builder resolvable item.
    /// </summary>
    internal sealed class ItemRegistration : ItemRegistrationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRegistration" /> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="inType">Type of the input.</param>
        internal ItemRegistration(ContainerBuilder builder, params Type[] inTypes)
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
