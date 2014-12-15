namespace MiniAutoFac.UnitTest.TestClasses
{
    using System;
    using MiniAutFac.Modules;

    public class ModuleWithResolveTracking : Module
    {
        public int Called { get; set; }

        public override void InstanceResolved(Type type, object instnace)
        {
            this.Called += 1;
        }
    }
}
