namespace MiniAutFac
{
    using System;
    using MiniAutFac.Parameters.Concrete;
    using Parameters;

    public static class ParametersExtensions
    {
        /// <summary>
        /// Withes the parameter.
        /// </summary>
        /// <param name="builderResolvableItem">The builder resolvable item.</param>
        /// <param name="paramter">The paramter.</param>
        /// <returns></returns>
        public static BuilderResolvableItemBase WithParameter(this BuilderResolvableItemBase builderResolvableItem,
                                                              Parameter paramter)
        {
            builderResolvableItem.Parameters.Add(paramter);
            return builderResolvableItem;
        }

        /// <summary>
        /// Withes the named parameter.
        /// </summary>
        /// <param name="builderResolvableItem">The builder resolvable item.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public static BuilderResolvableItemBase WithNamedParameter(this BuilderResolvableItemBase builderResolvableItem,
                                                                   string name, object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return builderResolvableItem.WithParameter(new NamedParameter(name, value));
        }

        /// <summary>
        /// Withes the evaluted paramter.
        /// </summary>
        /// <param name="builderResolvableItem">The builder resolvable item.</param>
        /// <param name="name">The name.</param>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public static BuilderResolvableItemBase WithEvalutedParamter(
            this BuilderResolvableItemBase builderResolvableItem, string name, Func<Type, object> instanceFactory)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return builderResolvableItem.WithParameter(new EvalutedParamter(name, instanceFactory));
        }
    }
}
