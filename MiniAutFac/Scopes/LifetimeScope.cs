namespace MiniAutFac.Scopes
{
    using System.Linq;
    using Interfaces;
    using System;
    using System.Collections.Generic;

    internal class LifetimeScope : ILifetimeScope
    {
        /// <summary>
        /// The container
        /// </summary>
        private Container container;

        /// <summary>
        /// The parent scope
        /// </summary>
        internal LifetimeScope ParentScope;

        /// <summary>
        /// The child scopes
        /// </summary>
        internal List<LifetimeScope> ChildScopes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LifetimeScope"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public LifetimeScope(LifetimeScope parent)
        {
            this.ParentScope = parent;
            this.ChildScopes = new List<LifetimeScope>();
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        /// <exception cref="System.ArgumentException">Not expected scope hierachy - cannot find origin container.</exception>
        internal Container Container
        {
            get
            {
                if (this.container != null)
                {
                    return this.container;
                }

                var scope = this;
                while (scope.ParentScope != null)
                {
                    scope = scope.ParentScope;
                }

                if (!(scope is Container))
                {
                    throw new ArgumentException("Not expected scope hierachy - cannot find origin container.");
                }

                this.container = scope as Container;
                return this.container;
            }
        }

        public virtual object Resolve(Type type)
        {
            if (!this.Container.TypeContainer.ContainsKey(type))
            {
                return this.Container.ResolveInternal(type, this);
            }

            var ctx = this.Container.TypeContainer[type];
            var outputType = ctx.First();

            var scope = ctx.Scopes[outputType];
            object instance;
            if (scope.GetInstance(this, out instance))
            {
                return instance;
            }

            instance = this.Container.ResolveInternal(type, this);
            scope.Resolved(this, type, instance);
            return instance;
        }

        public virtual void Dispose()
        {
            foreach (var childScope in this.ChildScopes)
            {
                childScope.Dispose();
            }
        }

        public virtual ILifetimeScope BeginLifetimeScope()
        {
            var newScope = new LifetimeScope(this);
            this.ChildScopes.Add(newScope);

            return newScope;
        }
    }
}
