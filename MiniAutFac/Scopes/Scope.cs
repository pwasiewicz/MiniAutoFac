namespace MiniAutFac.Scopes
{
    using System;

    public abstract class Scope
    {
        /// <summary>
        /// Gets the resolved instance within scope.
        /// </summary>
        /// <param name="scope">The lifetime scope within instance is needed.</param>
        /// <param name="valueFactory">The instance factory.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="value">The value instance.</param>
        public abstract void GetInstance(LifetimeScope scope, Func<object> valueFactory, Type valueType,
                                         out object value);
    }
}
