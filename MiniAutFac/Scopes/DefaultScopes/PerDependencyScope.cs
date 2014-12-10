namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class PerDependencyScope : Scope
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="value">The value.</param>
        public override void GetInstance(LifetimeScope scope, Func<object> valueFactory, Type valueType, out object value)
        {
            value = valueFactory();
        }
    }
}
