namespace MiniAutFac.Scopes
{
    using System;

    public abstract class Scope
    {
        /// <summary>
        /// Gets the resolved instance within lifetimeScope.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime lifetimeScope within instance is needed.</param>
        /// <param name="valueFactory">The instance factory.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="value">The value instance.</param>
        public abstract void GetInstance(LifetimeScope lifetimeScope, Func<object> valueFactory, Type valueType,
                                         out object value);
    }
}
