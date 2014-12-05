namespace MiniAutoFac.UnitTest.TestClasses
{
    class ParameterClassA
    {
        public readonly string Test;

        public ParameterClassA(string test)
        {
            this.Test = test;
        }
    }

    class ParameterClassB
    {
        public readonly ParameterClassA ClassA;

        public readonly int Test;

        public ParameterClassB(ParameterClassA classA, int test)
        {
            this.ClassA = classA;
            this.Test = test;
        }
    }
}
