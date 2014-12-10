namespace MiniAutFac.Parameters
{
    using System;
    using System.Reflection;

    public abstract class Parameter
    {
        /// <summary>
        /// Determines whether the specified ParamterInfo is applicable to current paramtere.
        /// </summary>
        /// <param name="pi">The paramter info.</param>
        /// <returns>True, if current parameter instance is applicable paramter info.</returns>
        public abstract bool IsApplicable(ParameterInfo pi);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="resolvedType">Type of the resolved.</param>
        /// <returns>Gets tha value of parameter.</returns>
        public abstract object GetValue(Type resolvedType);
    }
}
