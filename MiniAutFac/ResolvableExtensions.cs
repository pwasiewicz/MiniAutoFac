namespace MiniAutFac
{
    using System;
    using Interfaces;

    public static class ResolvableExtensions
    {
        /// <summary>
        /// Resolves the specified resolvable.
        /// </summary>
        /// <typeparam name="T">The typ to resolve.</typeparam>
        /// <param name="resolvable">The resolvable.</param>
        /// <returns>Resolved instance.</returns>
        /// <exception cref="System.ArgumentNullException">resolvable</exception>
        public static T Resolve<T>(this IResolvable resolvable)
        {
            if (resolvable == null)
            {
                throw new ArgumentNullException("resolvable");
            }

            return (T)resolvable.Resolve(typeof(T));
        }
    }
}
