namespace MiniAutFac.Resolvers
{
    using Extensions;
    using Interfaces;
    using Scopes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class EnumerableResolver : ConcreteResolverBase
    {
        /// <summary>
        /// Resolvables the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Resolvable(Type target, LifetimeScope lifetimeScope)
        {
            if (!target.IsGenericType)
            {
                return false;
            }

            var enumerableType = typeof(IEnumerable<>);
            return target.GetGenericTypeDefinition() == enumerableType &&
                   !lifetimeScope.Container.TypeContainer.ContainsKey(target);
        }

        /// <summary>
        /// Resolves the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="lifetimeScope">The lifetimeScope.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Fata error: List instance doesn't implements IList interface.</exception>
        /// <exception cref="System.NotSupportedException">Not supported yet.</exception>
        public override object Resolve(Type target, LifetimeScope lifetimeScope, object key = null)
        {
            var hiddenType = target.GetGenericArguments()[0];

            var outputList = lifetimeScope.Container.ActivationEngine(ActivatorDataForList(hiddenType)) as IList;
            if (outputList == null)
            {
                throw new InvalidOperationException("Fata error: List instance doesn't implements IList interface.");
            }

            if (lifetimeScope.Container.ResolveImplicit)
            {
                throw new NotSupportedException("Not supported yet.");
            }

            if (!lifetimeScope.Container.TypeContainer.ContainsKey(hiddenType))
            {
                throw new InvalidOperationException("Enumerable argument type not registered.");
            }

            var typeContext = lifetimeScope.Container.TypeContainer[hiddenType];
            foreach (var outputType in typeContext.GetForKey(key))
            {
                var scope = this.WrapScope(lifetimeScope, typeContext, typeContext.Scopes[outputType]);

                object instance;
                scope.GetInstance(lifetimeScope, () =>
                                                 {

                                                     var ctx = lifetimeScope.Container.TypeContainer[hiddenType];
                                                     return lifetimeScope.Container.CreateInstanceRecursive(lifetimeScope, ctx,
                                                                                                            outputType, target);
                                                 }, outputType, out instance);
                outputList.Add(instance);
            }

            return outputList;
        }

        /// <summary>
        /// Activators the data for list.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <returns></returns>
        private static IObjectActivatorData ActivatorDataForList(Type itemType)
        {
            var typeList = typeof(List<>).MakeGenericType(itemType);

            return new ObjectActivatorData
                   {
                       ConstructorArguments = Enumerable.Empty<object>(),
                       ConstructorInfo =
                           typeList.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                   .Single(ctor => !ctor.GetParameters().Any()),
                       ResolvedType = typeList
                   };
        }
    }
}
