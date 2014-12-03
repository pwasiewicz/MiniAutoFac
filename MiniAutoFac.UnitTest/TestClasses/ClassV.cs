namespace MiniAutoFac.UnitTest.TestClasses
{
    class ClassV
    {
        public InterfaceForClassB Instance { get; set; }

        public ClassV(InterfaceForClassB instance)
        {
            this.Instance = instance;
        }
    }
}
