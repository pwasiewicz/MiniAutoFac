namespace MiniAutoFac.UnitTest.TestClasses
{
    using MiniAutFac;
    using MiniAutFac.Modules;

    class EmbeddingModule : Module
    {
        public override void Registration(ContainerBuilder builder)
        {
            builder.Register<ClassB>().As<IFoo>();
            builder.RegisterModule(new SampleModule());
        }
    }
}
