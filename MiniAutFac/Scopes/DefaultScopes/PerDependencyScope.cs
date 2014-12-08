namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class PerDependencyScope : Scope
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="factory"></param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override void GetInstance(LifetimeScope scope, Func<object> factory, out object value)
        {
            value = factory();
        }
    }
}
