namespace MiniAutFac.Scopes
{
    using System;

    internal abstract class Scope
    {
        public abstract bool GetInstance(LifetimeScope scope, out object instance);

        public abstract void Resolved(LifetimeScope scope, Type outputType, object instance);
    }
}
