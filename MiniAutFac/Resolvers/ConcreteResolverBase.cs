namespace MiniAutFac.Resolvers
{
    using System;

    internal abstract class ConcreteResolverBase
    {
        protected readonly Container Container;

        protected ConcreteResolverBase(Container container)
        {
            this.Container = container;
        }

        public abstract bool Resolvable(Type target);
        public abstract object Resolve(Type target);
    }
}
