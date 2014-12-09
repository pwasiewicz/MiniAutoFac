namespace MiniAutoFac.UnitTest.TestClasses
{
    using MiniAutFac.Attributes;

    [ContainerType(typeof(ClassA))]
    class ClassA : IFoo
    {
    }


    class FooClass : IFoo
    {

    }
    interface IFoo
    {

    }
}
