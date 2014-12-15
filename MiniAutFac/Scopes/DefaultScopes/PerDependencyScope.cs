namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class PerDependencyScope : Scope
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="lifetimeScope">The lifetimeScope.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="value">The value.</param>
        public override void GetInstance(LifetimeScope lifetimeScope, Func<object> valueFactory, Type valueType, out object value)
        {
            value = valueFactory();
        }
    }
}
