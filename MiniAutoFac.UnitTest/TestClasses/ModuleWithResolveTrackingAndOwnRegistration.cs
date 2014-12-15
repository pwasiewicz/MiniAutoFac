namespace MiniAutoFac.UnitTest.TestClasses
{
    using System;
    using MiniAutFac;
    using MiniAutFac.Modules;

    public class ModuleWithResolveTrackingAndOwnRegistration : Module
    {
        public int Called { get; set; }

        public override void Registration(ContainerBuilder builder)
        {
            builder.Register<ClassA>().PerLifetimeScope();
        }

        public override void RegisteredInstanceResolved(Type type, object instnace)
        {

            this.Called += 1;
        }
    }
}
