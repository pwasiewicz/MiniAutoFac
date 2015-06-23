namespace MiniAutFac.Scopes
{
    using System.Linq.Expressions;
    using System.Reflection;
    using Interfaces;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LifetimeScope : ILifetimeScope
    {
        private static readonly Lazy<MethodInfo> ResolveMethodInfo =
            new Lazy<MethodInfo>(() => typeof(LifetimeScope).GetMethod("ResolveKeyed"));

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


        internal Delegate ResolvingDelegate(Type forType, object key)
        {
            if (forType == null) throw new ArgumentNullException("forType");

            var typeConst = Expression.Constant(forType);
            var keyConst = Expression.Constant(key);
            var lifetimeScopeInst = Expression.Constant(this);

            var callGetInst = Expression.Call(lifetimeScopeInst, ResolveMethodInfo.Value, typeConst, keyConst);
            var cast = Expression.Convert(callGetInst, forType);
            var valueFactory = Expression.Lambda(cast).Compile();

            return valueFactory;
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
            return Resolve(this, type, requestingType: null, key: null);
        }

        public object ResolveKeyed(Type type, object key)
        {
            return Resolve(this, type, requestingType: type, key: key);
        }

        internal virtual object Resolve(LifetimeScope lifetimeScope, Type type, Type requestingType, object key = null)
        {
            if (this.disposed)
            {
                throw new LifetimeScopeDisposedException();
            }

            if (!this.Container.TypeContainer.ContainsKey(type))
            {
                return this.Container.ResolveInternal(type, lifetimeScope, requestingType: requestingType, key: key);
            }

            var ctx = this.Container.TypeContainer[type];
            var outputType = ctx.First();

            var scope = this.Container.WrapScope(lifetimeScope, ctx, ctx.Scopes[outputType]);

            object instance;
            scope.GetInstance(this,
                              () =>
                              this.Container.ResolveInternal(type, lifetimeScope, requestingType: requestingType,
                                                             key: key), outputType, out instance);
            return instance;
        }

        public virtual void Dispose()
        {
            if (disposed)
            {
                throw new InvalidOperationException("Lifetime scope has already been disposed.");
            }

            foreach (var childScope in this.ChildScopes.Where(lifetimeScope => !lifetimeScope.disposed))
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
