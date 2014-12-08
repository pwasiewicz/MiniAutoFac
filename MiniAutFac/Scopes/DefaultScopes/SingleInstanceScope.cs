namespace MiniAutFac.Scopes.DefaultScopes
{
    using System;

    internal class SingleInstanceScope : Scope
    {
        private object instance;

        private readonly object lockObject = new object();

        public override void GetInstance(LifetimeScope scope, Func<object> valueFactory, out object value)
        {

            if (this.instance == null)
            {
                lock (this.lockObject)
                {
                    if (this.instance == null)
                    {
                        this.instance = valueFactory();
                    }
                }
            }

            value = this.instance;
        }
    }
}
