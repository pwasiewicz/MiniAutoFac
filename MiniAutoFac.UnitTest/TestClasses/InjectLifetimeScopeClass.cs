namespace MiniAutoFac.UnitTest.TestClasses
{
    using MiniAutFac.Interfaces;

    public class InjectLifetimeScopeClass
    {
        private readonly ILifetimeScope scope;
        public InjectLifetimeScopeClass(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public ILifetimeScope Scope
        {
            get { return this.scope; }
        }
    }
}
