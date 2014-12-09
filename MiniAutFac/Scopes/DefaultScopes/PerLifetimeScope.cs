namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;
    using System.Collections.Generic;

    internal class PerLifetimeScope : Scope
    {
        private readonly object lockObject = new object();

        /// <summary>
        /// The lifetime scopes and its instances of specified type.
        /// </summary>
        private readonly IDictionary<LifetimeScope, Dictionary<Type, object>> lifetimeScopes;

        /// <summary>
        /// Initializes a new value of the <see cref="PerLifetimeScope"/> class.
        /// </summary>
        public PerLifetimeScope()
        {
            this.lifetimeScopes = new Dictionary<LifetimeScope, Dictionary<Type, object>>();
        }

        public override void GetInstance(LifetimeScope scope, Func<object> valueFactory, Type valueType, out object value)
        {
            object result;

            lock (this.lockObject)
            {
                Dictionary<Type, object> scopeTypes;

                if (!this.lifetimeScopes.TryGetValue(scope, out scopeTypes))
                {
                    scopeTypes = new Dictionary<Type, object>();
                    this.lifetimeScopes.Add(scope, scopeTypes);
                }

                if (!scopeTypes.TryGetValue(valueType, out result))
                {
                    result = valueFactory();
                    scopeTypes.Add(valueType, result);
                }
            }

            value = result;
        }
    }
}
