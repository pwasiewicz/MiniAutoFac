namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class SingleInstanceScope : Scope
    {
        private object instance;

        public override void GetInstance(LifetimeScope scope, Func<object> factory, out object value)
        {
            if (this.instance == null)
            {
                this.instance = value = factory();
                return;
            }

            value = this.instance;
        }
    }
}
