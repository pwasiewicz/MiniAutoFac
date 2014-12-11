namespace MiniAutoFac.UnitTest.TestClasses
{
    using System;
    using MiniAutFac;
    using MiniAutFac.Modules;

    class ModuleWithRegisteredInstanceListener : Module
    {
        public override void Registration(ContainerBuilder builder)
        {
            builder.Register<ActivableClass>();
        }

        public override void InstanceActivated(Type type, object instance)
        {
            if (type == typeof(ActivableClass))
            {
                ((ActivableClass)instance).Activated = true;
            }
        }
    }

    public class ActivableClass
    {
        public bool Activated { get; set; }
    }
}
