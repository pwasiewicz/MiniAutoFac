namespace MiniAutFac.Resolvers
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    internal class EnumerableResolver : ConcreteResolverBase
    {
        public EnumerableResolver(Container container)
            : base(container)
        {
        }

        public override bool Resolvable(Type target)
        {
            if (!target.IsGenericType)
            {
                return false;
            }

            var enumerableType = typeof(IEnumerable<>);
            return target.GetGenericTypeDefinition() == enumerableType && !this.Container.TypeContainer.ContainsKey(target);
        }

        public override object Resolve(Type target)
        {
            var hiddenType = target.GetGenericArguments()[0];

            var outputList = this.Container.ActivationEngine(ActivatorDataForList(hiddenType)) as IList;
            if (outputList == null)
            {
                throw new InvalidOperationException("Fata error: List instance doesn't implements IList interface.");
            }

            if (this.Container.ResolveImplicit)
            {
                foreach (
                    var outputInstance in
                        this.Container.SearchImplicitImplementations(hiddenType)
                            .Select(outputType => this.Container.CreateInstanceRecursive(outputType)).Distinct())
                {
                    outputList.Add(outputInstance);
                }
            }
            else
            {
                foreach (
                    var outputInstance in
                        this.Container.TypeContainer[hiddenType]
                            .Select(outputType => this.Container.CreateInstanceRecursive(outputType)))
                {
                    outputList.Add(outputInstance);
                }
            }

            return outputList;
        }

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
