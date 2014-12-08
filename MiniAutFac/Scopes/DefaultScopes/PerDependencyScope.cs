namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class PerDependencyScope : Scope
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="valueFactory"></param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override void GetInstance(LifetimeScope scope, Func<object> valueFactory, out object value)
        {
            value = valueFactory();
        }
    }
}
