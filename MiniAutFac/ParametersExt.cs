namespace MiniAutFac
{
    using MiniAutFac.Interfaces;
    using MiniAutFac.Parameters.Concrete;
    using System;

    public static class ParametersExt
    {
        public static IBuilderResolvableItem WithNamedParameter(this IBuilderResolvableItem builderResolvableItem, string name, object value)
        {
            var rvbl = builderResolvableItem as BuilderResolvableItem;
            if (rvbl == null)
            {
                throw new NotSupportedException("Container builder used outside MiniAutFac");
            }

            rvbl.Parameters.Add(new NamedParameter(name, value));
            return rvbl;
        }
    }
}
