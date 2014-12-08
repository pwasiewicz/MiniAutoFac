namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class SingleInstanceScope : Scope
    {
        private object instance;

        public override bool GetInstance(LifetimeScope scope, out object instance)
        {
            if (this.instance == null)
            {
                instance = null;
                return false;
            }

            instance = this.instance;
            return true;
        }
        public override void Resolved(LifetimeScope scope, Type outputType, object instance)
        {
            if (this.instance != null)
            {
                throw new InvalidOperationException("Single instance is resolved more than once.");
            }

            this.instance = instance;
        }
    }
}
