namespace MiniAutFac
{
    using System;
    using Interfaces;
    using Parameters;

    public static class ResolvableExtensions
    {

        public static T Resolve<T>(this IResolvable resolvable, params Parameter[] additionalParameters)
        {
            if (resolvable == null)
            {
                throw new ArgumentNullException("resolvable");
            }

            return (T)resolvable.Resolve(typeof(T), additionalParameters);
        }
        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="resolvable">The resolvable implementation.</param>
        /// <returns>Resolved instance.</returns>
        /// <exception cref="System.ArgumentNullException">resolvable</exception>
        public static T Resolve<T>(this IResolvable resolvable)
        {
            if (resolvable == null)
            {
                throw new ArgumentNullException("resolvable");
            }

            return Resolve<T>(resolvable, new Parameter[0]);
        }

        public static T ResolveKeyed<T>(this IResolvable resolvable, object key)
        {
            if (resolvable == null)
            {
                throw new ArgumentNullException("resolvable");
            }

            return (T)resolvable.ResolveKeyed(typeof(T), key);
        }
    }
}
