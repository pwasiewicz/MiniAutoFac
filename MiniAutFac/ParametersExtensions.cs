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
        /// <param name="itemRegistration">The builder resolvable item.</param>
        /// <param name="paramter">The paramter.</param>
        /// <returns></returns>
        public static ItemRegistrationBase WithParameter(this ItemRegistrationBase itemRegistration,
                                                              Parameter paramter)
        {
            itemRegistration.Parameters.Add(paramter);
            return itemRegistration;
        }

        /// <summary>
        /// Withes the named parameter.
        /// </summary>
        /// <param name="itemRegistration">The builder resolvable item.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public static ItemRegistrationBase WithNamedParameter(this ItemRegistrationBase itemRegistration,
                                                                   string name, object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return itemRegistration.WithParameter(new NamedParameter(name, value));
        }

        /// <summary>
        /// Withes the evaluted paramter.
        /// </summary>
        /// <param name="itemRegistration">The builder resolvable item.</param>
        /// <param name="name">The name.</param>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public static ItemRegistrationBase WithEvalutedParamter(
            this ItemRegistrationBase itemRegistration, string name, Func<Type, object> instanceFactory)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return itemRegistration.WithParameter(new EvalutedParamter(name, instanceFactory));
        }
    }
}
