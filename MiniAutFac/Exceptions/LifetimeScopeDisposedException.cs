namespace MiniAutFac.Exceptions
{
    using System;

    public class LifetimeScopeDisposedException : Exception
    {
        public LifetimeScopeDisposedException() : base("The lifetime scope is dispoed. Cannot resolve instances.")
        {
        }
    }
}
