namespace MiniAutFac.Interfaces
{
    using System;

    public interface ILifetimeScope : IResolvable, IDisposable
    {
        /// <summary>
        /// Begins the lifetime scope.
        /// </summary>
        /// <returns>Life time scope.</returns>
        ILifetimeScope BeginLifetimeScope();
    }
}
