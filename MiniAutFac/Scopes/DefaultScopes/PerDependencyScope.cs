namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class PerDependencyScope : Scope
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public override bool GetInstance(LifetimeScope scope, out object instance)
        {
            instance = null;
            return false;
        }

        /// <summary>
        /// Resolveds the specified output type.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="outputType">Type of the output.</param>
        /// <param name="instance">The instance.</param>
        public override void Resolved(LifetimeScope scope, Type outputType, object instance)
        {
        }
    }
}
