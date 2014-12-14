namespace MiniAutoFac.UnitTest.TestClasses.EnumerableBug
{
    using System;
    using System.Collections.Generic;

    public class SomeContext
    {
        public IEnumerable<SomeDisposable> Disposables { get; set; }
        public SomeContext(IEnumerable<SomeDisposable> disposables)
        {
            this.Disposables = disposables;
        }
    }

    public class SomeDisposable : IDisposable
    {
        public bool Disposed { get; set; }
        public void Dispose()
        {
            this.Disposed = true;
        }
    }
}
