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
    using Exceptions;
    using Parameters;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ItemRegistration : ItemRegistration<object>
    {
        internal ItemRegistration(ContainerBuilder builder, params Type[] inTypes) : base(builder, inTypes) {}
    }

    /// <summary>
    /// The builder resolvable item.
    /// </summary>
    internal class ItemRegistration<TConcreteType> : ConcreteItemRegistrationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRegistration" /> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="inTypes"></param>
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
