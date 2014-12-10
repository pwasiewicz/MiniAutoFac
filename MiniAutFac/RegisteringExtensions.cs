namespace MiniAutFac
{
    using System;
    using System.Linq;

    public static class RegisteringExtensions
    {
        /// <summary>
        /// Registers type as all interfaces that it implements.
        /// </summary>
        /// <param name="resolvableItemRegistration">The resolvable item builder.</param>
        /// <returns>The resolvable item builder.</returns>
        public static ItemRegistrationBase AsImplementedInterfaces(this ItemRegistrationBase resolvableItemRegistration)
        {
            foreach (var inType in resolvableItemRegistration.InTypes)
            {
                var interfaces = inType.GetInterfaces();
                if (!interfaces.Any())
                {
                    resolvableItemRegistration.AsType = null;
                    return resolvableItemRegistration;
                }

                resolvableItemRegistration.AsType = interfaces.First();

                foreach (var @interface in interfaces.Skip(1))
                {
                    resolvableItemRegistration.Origin.Register(inType).As(@interface);
                }
            }

            return resolvableItemRegistration;
        }

        /// <summary>
        /// Registers type as generic argument.
        /// </summary>
        /// <typeparam name="T">The type, that type should be registered as.</typeparam>
        /// <param name="resolvableItemRegistrationBase">The resolvable item base.</param>
        /// <returns>The resolvable item base.</returns>
        public static ItemRegistrationBase As<T>(this ItemRegistrationBase resolvableItemRegistrationBase)
        {
            resolvableItemRegistrationBase.As(typeof(T));
            return resolvableItemRegistrationBase;
        }

        /// <summary>
        /// Adds function that creates instances of registered type.
        /// </summary>
        /// <param name="resolvableItemRegistrationBase">The resolvable item base.</param>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <returns>Type registration base.</returns>
        public static ItemRegistrationBase As(this ItemRegistrationBase resolvableItemRegistrationBase,
                                                   Func<object> instanceFactory)
        {
            resolvableItemRegistrationBase.OwnFactory = instanceFactory;
            return resolvableItemRegistrationBase;
        }

        /// <summary>
        ///  Adds the instane of registered type that will be used when resolving.
        /// </summary>
        /// <param name="resolvableItemRegistrationBase">The resolvable item registration base.</param>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <returns></returns>
        public static ItemRegistrationBase As(this ItemRegistrationBase resolvableItemRegistrationBase,
                                           object instanceFactory)
        {
            resolvableItemRegistrationBase.OwnFactory = () => instanceFactory;
            return resolvableItemRegistrationBase;
        }
    }
}
