namespace MiniAutFac
{
    using Scopes.DefaultScopes;

    public static class ScopeExtensions
    {
        /// <summary>
        /// Default. Every instance request will produce new instance.
        /// </summary>
        /// <param name="resolvable">The resolvable builder.</param>
        /// <returns>The resolvable builder.</returns>
        public static BuilderResolvableItemBase PerDependency(this BuilderResolvableItemBase resolvable)
        {
            resolvable.Scope = new PerDependencyScope();
            return resolvable;
        }

        /// <summary>
        /// Registered instance will live only within liftimescope.
        /// </summary>
        /// <param name="resolvable">The resolvable builder.</param>
        /// <returns>The resolvable builder.</returns>
        public static BuilderResolvableItemBase PerLifetimeScope(this BuilderResolvableItemBase resolvable)
        {
            resolvable.Scope = new PerLifetimeScope();
            return resolvable;
        }

        /// <summary>
        /// Registered instance will have only one instance per container including nested liftime scopes.
        /// </summary>
        /// <param name="resolvable">The resolvable builder.</param>
        /// <returns>The resolvable builder.</returns>
        public static BuilderResolvableItemBase SingleInstance(this BuilderResolvableItemBase resolvable)
        {
            resolvable.Scope = new SingleInstanceScope();
            return resolvable;
        }
    }
}
