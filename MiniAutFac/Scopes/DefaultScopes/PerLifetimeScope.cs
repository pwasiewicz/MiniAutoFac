namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;
    using System.Collections.Generic;

    internal class PerLifetimeScope : Scope
    {
        /// <summary>
        /// The lifetime scopes
        /// </summary>
        private IDictionary<LifetimeScope, object> lifetimeScopes;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerLifetimeScope"/> class.
        /// </summary>
        public PerLifetimeScope()
        {
            this.lifetimeScopes = new Dictionary<LifetimeScope, object>();
        }


        public override bool GetInstance(LifetimeScope scope, out object instance)
        {
            instance = null;

            if (!this.lifetimeScopes.ContainsKey(scope))
            {
                return false;
            }

            if (this.lifetimeScopes[scope] == null)
            {
                return false;
            }

            instance = this.lifetimeScopes[scope];
            return true;
        }

        public override void Resolved(LifetimeScope scope, Type outputType, object instance)
        {
            if (this.lifetimeScopes.ContainsKey(scope))
            {
                if (this.lifetimeScopes[scope] != null)
                {
                    throw new InvalidOperationException("Invalid scope resolution.");
                }

                this.lifetimeScopes[scope] = instance;
            }
            else
            {
                this.lifetimeScopes.Add(scope, instance);
            }
        }
    }
}
