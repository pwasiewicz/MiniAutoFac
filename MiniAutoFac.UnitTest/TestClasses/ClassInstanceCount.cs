namespace MiniAutoFac.UnitTest.TestClasses
{
    public class ClassInstanceCount
    {
        public static int Instances;

        public ClassInstanceCount()
        {
            Instances += 1;
        }
    }
}
