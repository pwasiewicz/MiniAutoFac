namespace MiniAutoFac.UnitTest.TestClasses
{
    class Class2
    {
        public Class1 Class1 { get; set; }
        public Class2(Class1 class1)
        {
            this.Class1 = class1;
        }
    }
}
