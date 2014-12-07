namespace MiniAutFac
{
    using System.Linq;

    public static class ResolvableItemExtensions
    {
        public static BuilderResolvableItemBase AsImplementedInterfaces(this BuilderResolvableItemBase resolvableItem)
        {
            var interfaces = resolvableItem.InType.GetInterfaces();
            if (!interfaces.Any())
            {
                resolvableItem.AsType = null;
                return resolvableItem;
            }

            resolvableItem.AsType = interfaces.First();

            foreach (var @interface in interfaces.Skip(1))
            {
                resolvableItem.Origin.Register(resolvableItem.InType).As(@interface);
            }

            return resolvableItem;
        }

        /// <summary>
        /// Ases the specified resolvable item base.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resolvableItemBase">The resolvable item base.</param>
        /// <returns></returns>
        public static BuilderResolvableItemBase As<T>(this BuilderResolvableItemBase resolvableItemBase)
        {
            resolvableItemBase.As(typeof (T));
            return resolvableItemBase;
        }
    }
}
