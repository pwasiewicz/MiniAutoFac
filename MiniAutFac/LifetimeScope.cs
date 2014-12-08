namespace MiniAutFac
{
    using System.Linq;
    using Interfaces;
    using System;
    using System.Collections.Generic;

    internal class LifetimeScope : ILifetimeScope
    {
        public LifetimeScope(LifetimeScope parentScope)
        {
            this.ParentScope = parentScope;
            this.ChildScopes = new List<LifetimeScope>();
            this.ScopeInstances = new Dictionary<Type, LifetimeScopeResolvedContext>();
        }

        internal LifetimeScope ParentScope { get; set; }

        internal IList<LifetimeScope> ChildScopes { get; set; }

        internal Dictionary<Type, LifetimeScopeResolvedContext> ScopeInstances { get; set; }

        private Container container;

        internal Container Container
        {
            get
            {
                if (this.container != null)
                {
                    return this.container;
                }

                var scope = this;

                // TODO circ check
                while (scope.ParentScope != null)
                {
                    scope = scope.ParentScope;
                }

                if (!(scope is Container))
                {
                    throw new InvalidOperationException("Invalid lifetime scope hierarchy.");
                }

                this.container = scope as Container;
                return this.container;
            }
        }

        private IEnumerable<IDisposable> FetchAllDisposablesFromScope()
        {
            return this.ScopeInstances.SelectMany(pair => pair.Value.ScopeInstances.Values).OfType<IDisposable>();
        }

        public virtual void Dispose()
        {
            foreach (var childScope in this.ChildScopes)
            {
                childScope.Dispose();
            }

            foreach (var disposable in this.FetchAllDisposablesFromScope())
            {
                disposable.Dispose();
            }
        }

        public virtual object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public virtual T Resolve<T>()
        {
            throw new System.NotImplementedException();
        }

        public virtual void BeginLifetimeScope()
        {
            throw new System.NotImplementedException();
        }
    }


    internal class LifetimeScopeResolvedContext
    {
        internal IDictionary<Type, object> ScopeInstances;

        public LifetimeScopeResolvedContext()
        {
            this.ScopeInstances = new Dictionary<Type, object>();
        }
    }
}
