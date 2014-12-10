namespace MiniAutFac
{
    using Scopes;
    using Scopes.DefaultScopes;

    public static class ScopeExtensions
    {
        /// <summary>
        /// Withes the scope.
        /// </summary>
        /// <param name="resolvable">The resolvable.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static BuilderResolvableItemBase WithScope(this BuilderResolvableItemBase resolvable, Scope scope)
        {
            resolvable.Scope = scope;
            return resolvable;
        }

        /// <summary>
        /// Default. Every instance request will produce new instance.
        /// </summary>
        /// <param name="resolvable">The resolvable builder.</param>
        /// <returns>The resolvable builder.</returns>
        public static BuilderResolvableItemBase PerDependency(this BuilderResolvableItemBase resolvable)
        {
            return resolvable.WithScope(new PerDependencyScope());
        }

        /// <summary>
        /// Registered instance will live only within liftimescope.
        /// </summary>
        /// <param name="resolvable">The resolvable builder.</param>
        /// <returns>The resolvable builder.</returns>
        public static BuilderResolvableItemBase PerLifetimeScope(this BuilderResolvableItemBase resolvable)
        {
            return resolvable.WithScope(new PerLifetimeScope());
        }

        /// <summary>
        /// Registered instance will have only one instance per container including nested liftime scopes.
        /// </summary>
        /// <param name="resolvable">The resolvable builder.</param>
        /// <returns>The resolvable builder.</returns>
        public static BuilderResolvableItemBase SingleInstance(this BuilderResolvableItemBase resolvable)
        {
            return resolvable.WithScope(new SingleInstanceScope());
        }
    }
}
