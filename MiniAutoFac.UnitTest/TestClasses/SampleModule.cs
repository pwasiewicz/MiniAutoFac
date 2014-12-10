namespace MiniAutoFac.UnitTest.TestClasses
{
    using MiniAutFac;
    using MiniAutFac.Modules;

    class SampleModule : Module
    {
        public override void Registration(ContainerBuilder builder)
        {
            builder.Register<ClassB>().As<ClassA>();
        }
    }
}
