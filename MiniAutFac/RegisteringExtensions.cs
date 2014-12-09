namespace MiniAutFac
{
    using System.Linq;

    public static class RegisteringExtensions
    {
        /// <summary>
        /// Registers type as all interfaces that it implements.
        /// </summary>
        /// <param name="resolvableItem">The resolvable item builder.</param>
        /// <returns>The resolvable item builder.</returns>
        public static BuilderResolvableItemBase AsImplementedInterfaces(this BuilderResolvableItemBase resolvableItem)
        {
            foreach (var inType in resolvableItem.InTypes)
            {
                var interfaces = inType.GetInterfaces();
                if (!interfaces.Any())
                {
                    resolvableItem.AsType = null;
                    return resolvableItem;
                }

                resolvableItem.AsType = interfaces.First();

                foreach (var @interface in interfaces.Skip(1))
                {
                    resolvableItem.Origin.Register(inType).As(@interface);
                }
            }

            return resolvableItem;
        }

        /// <summary>
        /// Registers type as generic argument.
        /// </summary>
        /// <typeparam name="T">The type, that type should be registered as.</typeparam>
        /// <param name="resolvableItemBase">The resolvable item base.</param>
        /// <returns>The resolvable item base.</returns>
        public static BuilderResolvableItemBase As<T>(this BuilderResolvableItemBase resolvableItemBase)
        {
            resolvableItemBase.As(typeof(T));
            return resolvableItemBase;
        }
    }
}
