namespace MiniAutFac.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ContainerExtensions
    {
        public static IEnumerable<Type> SearchImplicitImplementations(this Container container, Type type)
        {
            return container.TypeContainer.SelectMany(pair => pair.Value.Where(type.IsAssignableFrom));
        }
    }
}
