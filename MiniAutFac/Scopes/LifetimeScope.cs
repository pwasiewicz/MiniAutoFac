namespace MiniAutFac.Scopes
{
    using System.Linq;
    using Interfaces;
    using System;
    using System.Collections.Generic;

    internal class LifetimeScope : ILifetimeScope
    {
        private Container container;

        internal LifetimeScope ParentScope;

        internal List<LifetimeScope> ChildScopes;

        public LifetimeScope(LifetimeScope parent)
        {
            this.ParentScope = parent;
            this.ChildScopes = new List<LifetimeScope>();
        }

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
            scope.GetInstance(this, () => this.Container.ResolveInternal(type, this), out instance);
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
