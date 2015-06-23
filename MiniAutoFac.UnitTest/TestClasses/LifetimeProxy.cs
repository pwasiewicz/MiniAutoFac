namespace MiniAutoFac.UnitTest.TestClasses
{
    using System;
    using MiniAutFac.Interfaces;

    public class LifetimeProxy : ILifetimeScope
    {
        public object Resolve(Type type)
        {
            return null;
        }

        public void Dispose()
        {
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return this;
        }
    }
}
