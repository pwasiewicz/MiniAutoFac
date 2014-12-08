namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;
    using System.Collections.Generic;

    internal class PerLifetimeScope : Scope
    {
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

        public override void GetInstance(LifetimeScope scope, Func<object> factory, out object value)
        {
            if (this.lifetimeScopes.ContainsKey(scope))
            {
                value = this.lifetimeScopes[scope];
                return;
            }

            value = factory();
            this.lifetimeScopes.Add(scope, value);
        }
    }
}
