namespace MiniAutFac.Scopes
{
    using System;

    internal abstract class Scope
    {
        public abstract void GetInstance(LifetimeScope scope, Func<object> valueFactory, Type valueType, out object value);
    }
}
