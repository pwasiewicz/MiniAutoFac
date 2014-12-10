namespace MiniAutFac.Scopes
{
    using System.Linq;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using MiniAutFac.Exceptions;

    public class LifetimeScope : ILifetimeScope
    {
        private Container container;

        internal LifetimeScope ParentScope;

        internal List<LifetimeScope> ChildScopes;

        internal HashSet<object> ScopeAllInstances;

        private bool disposed;

        public LifetimeScope(LifetimeScope parent)
        {
            this.ParentScope = parent;
            this.ChildScopes = new List<LifetimeScope>();
            this.ScopeAllInstances = new HashSet<object>();
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
            if (this.disposed)
            {
                throw new LifetimeScopeDisposedException();
            }

            if (!this.Container.TypeContainer.ContainsKey(type))
            {
                return this.Container.ResolveInternal(type, this);
            }

            var ctx = this.Container.TypeContainer[type];
            var outputType = ctx.First();

            var scope = ctx.Scopes[outputType];

            object instance;
            scope.GetInstance(this, () => this.Container.ResolveInternal(type, this), outputType, out instance);
            return instance;
        }

        public virtual void Dispose()
        {
            foreach (var childScope in this.ChildScopes)
            {
                childScope.Dispose();
            }

            foreach (var disposable in this.ScopeAllInstances.OfType<IDisposable>())
            {
                disposable.Dispose();
            }

            this.disposed = true;
        }

        public virtual ILifetimeScope BeginLifetimeScope()
        {
            var newScope = new LifetimeScope(this);
            this.ChildScopes.Add(newScope);

            return newScope;
        }
    }
}
