namespace MiniAutFac.Resolvers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Interfaces;
    using Scopes;

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

            var typeConst = Expression.Constant(hiddenType);
            var lifetimeScopeInst = Expression.Constant(lifetimeScope);

            var resolveMth = typeof(LifetimeScope).GetMethod("Resolve");

            var callGetInst = Expression.Call(lifetimeScopeInst, resolveMth, typeConst);
            var cast = Expression.Convert(callGetInst, hiddenType);
            var valueFactory = Expression.Lambda(cast).Compile();

            return
                lifetimeScope.Container.ActivationEngine(ActivatorDataForList(hiddenType, valueFactory));
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
