namespace MiniAutFac.Resolvers
{
    using System;
    using Context;
    using Scopes;

    internal abstract class ConcreteResolverBase
    {
        /// <summary>
        /// Resolvables the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <returns></returns>
        public abstract bool Resolvable(Type target, LifetimeScope lifetimeScope);

        /// <summary>
        /// Resolves the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="lifetimeScope">The lifetimeScope.</param>
        /// <returns></returns>
        public abstract object Resolve(Type target, LifetimeScope lifetimeScope);

        /// <summary>
        /// Wraps the scope.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="desiredScope">The desired scope.</param>
        /// <returns></returns>
        protected Scope WrapScope(LifetimeScope lifetimeScope, RegisteredTypeContext ctx, Scope desiredScope)
        {
            return lifetimeScope.Container.WrapScope(lifetimeScope, ctx, desiredScope);
        }
    }
}
