namespace MiniAutFac.Parameters.Concrete
{
    using System;
    using System.Reflection;

    internal class EvalutedParamter : Parameter
    {
        /// <summary>
        /// The name
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The evaluator
        /// </summary>
        private readonly Func<Type, object> evaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvalutedParamter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="evaluator">The evaluator.</param>
        public EvalutedParamter(string name, Func<Type, object> evaluator)
        {
            this.name = name;
            this.evaluator = evaluator;
        }

        /// <summary>
        /// Determines whether the specified pi is applicable.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public override bool IsApplicable(ParameterInfo pi)
        {
            return pi.Name == name;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="resolvedType">Type of the resolved.</param>
        /// <returns></returns>
        public override object GetValue(Type resolvedType)
        {
            return this.evaluator(resolvedType);
        }
    }
}
