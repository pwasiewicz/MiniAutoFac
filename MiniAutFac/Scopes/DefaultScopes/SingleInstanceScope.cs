namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;
    using System.Collections.Generic;

    internal class SingleInstanceScope : Scope
    {
        private readonly Dictionary<Type, object> instances;

        private readonly object lockObject = new object();

        public SingleInstanceScope()
        {
            this.instances = new Dictionary<Type, object>();
        }

        public override void GetInstance(LifetimeScope lifetimeScope, Func<object> valueFactory, Type valueType, out object value)
        {
            object result;

            lock (this.lockObject)
            {
                if (!this.instances.TryGetValue(valueType, out result))
                {
                    var newInstance = valueFactory();
                    this.instances.Add(valueType, newInstance);
                    result = newInstance;
                }
            }

            value = result;
        }
    }
}
