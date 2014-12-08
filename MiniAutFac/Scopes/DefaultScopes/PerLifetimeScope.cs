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
        private readonly IDictionary<LifetimeScope, object> lifetimeScopes;

        /// <summary>
        /// Initializes a new value of the <see cref="PerLifetimeScope"/> class.
        /// </summary>
        public PerLifetimeScope()
        {
            this.lifetimeScopes = new Dictionary<LifetimeScope, object>();
        }

        public override void GetInstance(LifetimeScope scope, Func<object> valueFactory, out object value)
        {
            object result;
            lock (this.lockObject)
            {
                if (!this.lifetimeScopes.TryGetValue(scope, out result))
                {
                    result = valueFactory();
                    this.lifetimeScopes.Add(scope, result);
                }                
            }

            value = result;
        }
    }
}
