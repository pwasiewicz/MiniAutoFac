namespace MiniAutFac.Resolvers
{
    using System;
    using Interfaces;
    using Scopes;

    internal class ContainerItselfResolver : ConcreteResolverBase
    {
        public override bool Resolvable(Type target, LifetimeScope lifetimeScope)
        {
            return typeof(ILifetimeScope) == target;
        }

        public override object Resolve(Type target, LifetimeScope lifetimeScope, object key = null)
        {
            return lifetimeScope;
        }
    }
}
