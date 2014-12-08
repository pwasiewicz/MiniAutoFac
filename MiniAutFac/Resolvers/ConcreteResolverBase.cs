namespace MiniAutFac.Resolvers
{
    using System;
    using Scopes;

    internal abstract class ConcreteResolverBase
    {
        /// <summary>
        /// Resolvables the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public abstract bool Resolvable(Type target, LifetimeScope scope);

        /// <summary>
        /// Resolves the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="lifetimeScope">The lifetimeScope.</param>
        /// <returns></returns>
        public abstract object Resolve(Type target, LifetimeScope lifetimeScope);
    }
}
