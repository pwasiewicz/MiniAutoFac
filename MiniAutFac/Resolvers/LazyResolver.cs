namespace MiniAutFac.Resolvers
{
    using Interfaces;
    using Scopes;
    using System;
    using System.Linq;
    using System.Reflection;

    internal class LazyResolver : ConcreteResolverBase
    {
        public override bool Resolvable(Type target, LifetimeScope lifetimeScope)
        {
            if (!target.IsGenericType)
            {
                return false;
            }

            var enumerableType = typeof(Lazy<>);
            return target.GetGenericTypeDefinition() == enumerableType &&
                   !lifetimeScope.Container.TypeContainer.ContainsKey(target);
        }

        public override object Resolve(Type target, LifetimeScope lifetimeScope)
        {
            var hiddenType = target.GetGenericArguments()[0];

            return
                lifetimeScope.Container.ActivationEngine(ActivatorDataForList(hiddenType,
                                                                              lifetimeScope.ResolvingDelegate(hiddenType)));
        }

        private static IObjectActivatorData ActivatorDataForList(Type itemType, Delegate valueFactory)
        {
            var typeList = typeof(Lazy<>).MakeGenericType(itemType);

            return new ObjectActivatorData
                   {
                       ConstructorArguments = new[] { valueFactory },
                       ConstructorInfo =
                           typeList.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                   .First(
                                           ctor =>
                                           ctor.GetParameters().Count() == 1),
                       ResolvedType = typeList
                   };

        }
    }
}
