namespace MiniAutoFac.UnitTest.TestClasses
{
    using System;

    internal class DisposableClass : IDisposable
    {
        public bool DisposeCalled { get; set; }

        public DisposableClass()
        {
            this.DisposeCalled = false;
        }

        public void Dispose()
        {
            this.DisposeCalled = true;
        }
    }
}
