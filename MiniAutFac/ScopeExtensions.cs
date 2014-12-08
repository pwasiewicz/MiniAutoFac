namespace MiniAutFac
{
    using Scopes.DefaultScopes;

    public static class ScopeExtensions
    {
        /// <summary>
        /// Pers the dependency.
        /// </summary>
        /// <param name="resolvable">The resolvable.</param>
        /// <returns></returns>
        public static BuilderResolvableItemBase PerDependency(this BuilderResolvableItemBase resolvable)
        {
            resolvable.Scope = new PerDependencyScope();
            return resolvable;
        }

        /// <summary>
        /// Pers the lifetime scope.
        /// </summary>
        /// <param name="resolvable">The resolvable.</param>
        /// <returns></returns>
        public static BuilderResolvableItemBase PerLifetimeScope(this BuilderResolvableItemBase resolvable)
        {
            resolvable.Scope = new PerLifetimeScope();
            return resolvable;
        }

        /// <summary>
        /// Singles the instance.
        /// </summary>
        /// <param name="resolvable">The resolvable.</param>
        /// <returns></returns>
        public static BuilderResolvableItemBase SingleInstance(this BuilderResolvableItemBase resolvable)
        {
            resolvable.Scope = new SingleInstanceScope();
            return resolvable;
        }
    }
}
