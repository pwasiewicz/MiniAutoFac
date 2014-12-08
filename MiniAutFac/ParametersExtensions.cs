namespace MiniAutFac
{
    using System;
    using MiniAutFac.Parameters.Concrete;

    public static class ParametersExtensions
    {
        /// <summary>
        /// Withes the named parameter.
        /// </summary>
        /// <param name="builderResolvableItem">The builder resolvable item.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static BuilderResolvableItemBase WithNamedParameter(this BuilderResolvableItemBase builderResolvableItem,
                                                                   string name, object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            builderResolvableItem.Parameters.Add(new NamedParameter(name, value));
            return builderResolvableItem;
        }
    }
}
