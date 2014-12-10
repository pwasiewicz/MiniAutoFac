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
    using System;
    using System.Collections.Generic;
    using MiniAutFac.Parameters;
    using Scopes;
    using Scopes.DefaultScopes;

    /// <summary>
    /// The BuilderResolvableItem interface.
    /// </summary>
    public abstract class BuilderResolvableItemBase
    {
        internal readonly ContainerBuilder Origin;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderResolvableItemBase"/> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        protected BuilderResolvableItemBase(ContainerBuilder origin)
        {
            this.Origin = origin;
            this.Scope = new PerDependencyScope();
        }

        /// <summary>
        /// Gets or sets the registered type.
        /// </summary>
        /// <value> The type of the input.  </value>
        internal virtual Type[] InTypes { get; set; }

        /// <summary>
        /// Gets or sets the output type.
        /// </summary>
        /// <value>The type of the output. </value>
        internal virtual Type AsType { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        internal virtual List<Parameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the own factory.
        /// </summary>
        internal virtual Func<object> OwnFactory { get; set; } 

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        internal virtual Scope Scope { get; set; }

        /// <summary>
        /// Determines the output type of registered type with builder.
        /// </summary>
        /// <param name="type">The type.</param>
        public abstract void As(Type type);
    }
}
