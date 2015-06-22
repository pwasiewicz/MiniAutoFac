// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuilderResolvableItem.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the IBuilderResolvableItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac
{
    using MiniAutFac.Context;
    using MiniAutFac.Parameters;
    using Modules;
    using Scopes;
    using Scopes.DefaultScopes;
    using System;
    using System.Collections.Generic;

    public abstract class ConcreteItemRegistrationBase : ItemRegistrationBase
    {
        protected ConcreteItemRegistrationBase(ContainerBuilder origin) : base(origin) { }
    }

    /// <summary>
    /// The ItemRegistration interface.
    /// </summary>
    public abstract class ItemRegistrationBase
    {
        internal readonly ContainerBuilder Origin;
        private Scope scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRegistrationBase"/> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        protected ItemRegistrationBase(ContainerBuilder origin)
        {
            this.Origin = origin;
            this.scope = new PerDependencyScope();
        }

        /// <summary>
        /// Gets or sets the registered type.
        /// </summary>
        /// <value> The type of the input.  </value>
        internal Type[] InTypes { get; set; }

        /// <summary>
        /// Gets or sets the output type.
        /// </summary>
        /// <value>The type of the output. </value>
        internal Type AsType { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        internal List<Parameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the own factory.
        /// </summary>
        internal virtual Func<ActivationContext, object> OwnFactory { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        internal virtual Scope Scope
        {
            get { return this.scope; }
            set { this.scope = value; }
        }

        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        internal virtual Module Module { get; set; }

        /// <summary>
        /// Determines the output type of registered type with builder.
        /// </summary>
        /// <param name="type">The type.</param>
        public abstract void As(Type type);
    }
}
