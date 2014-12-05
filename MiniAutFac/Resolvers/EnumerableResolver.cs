namespace MiniAutFac.Resolvers
{
    using System;
    using System.Collections.Generic;

    internal class EnumerableResolver : ConcreteResolverBase
    {
        public EnumerableResolver(Container container) : base(container)
        {
        }

        public override bool Resolvable(Type target)
        {
            var enumerableType = typeof(IEnumerable<>);
            return target == enumerableType && !this.Container.TypeContainer.ContainsKey(target);
        }

        public override object Resolve(Type target)
        {
            throw new NotImplementedException();
        }
    }
}
