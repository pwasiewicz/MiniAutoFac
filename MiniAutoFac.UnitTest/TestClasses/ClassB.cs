namespace MiniAutoFac.UnitTest.TestClasses
{
    using MiniAutFac.Attributes;

    [ContainerType(typeof(InterfaceForClassB))]
    [ContainerType(typeof(ClassA))]
    class ClassB: ClassA, InterfaceForClassB
    {
    }
}
