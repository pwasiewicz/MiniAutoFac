// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Container.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   Defines the Container type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MiniAutFac.Exceptions;
    using MiniAutFac.Interfaces;

    /// <summary>
    /// The default container - instance factory.
    /// </summary>
    internal class Container : IResolvable 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        public Container()
        {
            this.ResolveImplicit = false;
        }

        /// <summary>
        /// Gets or sets the type container.
        /// </summary>
        internal IDictionary<Type, Type> TypeContainer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether resolve implicit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [resolve implicit]; otherwise, <c>false</c>.
        /// </value>
        internal bool ResolveImplicit { get; set; }

        /// <summary>
        /// Gets or sets the activation engine.
        /// </summary>
        internal Func<IObjectActivatorData, object> ActivationEngine { get; set; } 

        /// <summary>
        /// Creates the instance of type T.
        /// </summary>
        /// <typeparam name="T">Type to resolve.</typeparam>
        /// <returns> The <see cref="T"/>.  </returns>
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        /// <summary>
        /// Resolves the instance of type T.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <typeparam name="T">Instance of type to create.</typeparam>
        /// <returns> New instance of type T. </returns>
        public object Resolve(Type type)
        {
            if (this.TypeContainer == null)
            {
                throw new TypeRepositoryEmptyException();
            }

            var desiredType = type;
            var outputPair = this.TypeContainer.FirstOrDefault(pair => pair.Key == desiredType);
            if (IsPairValuesNull(outputPair))
            {
                if (!this.ResolveImplicit)
                {
                    throw new CannotResolveTypeException();
                }


                outputPair = this.TypeContainer.FirstOrDefault(pair => desiredType.IsAssignableFrom(pair.Value));
                if (IsPairValuesNull(outputPair))
                {
                    throw new CannotResolveTypeException();
                }
            }

            var outputType = outputPair.Value;
            if (!desiredType.IsAssignableFrom(outputType))
            {
                throw new CannotResolveTypeException();
            }

            LinkedList<object> constructorArguments = null;

            var constructors = outputType.GetConstructors();
            ConstructorInfo ctor = null;
            if (constructors.Any())
            {
                foreach (var constructorInfo in constructors)
                {
                    var parametersInsance = new LinkedList<object>();
                    if (!this.ResolveConstructorParameters(constructorInfo, parametersInsance))
                    {
                        continue;
                    }

                    constructorArguments = parametersInsance;
                    ctor = constructorInfo;
                    break;
                }
            }

            if (constructorArguments == null || ctor == null)
            {
                throw new NotAssignableException();
            }

            return
                this.ActivationEngine(
                    new ObjectActivatorData
                        {
                            ResolvedType = outputType,
                            ConstructorInfo = ctor,
                            ConstructorArguments = constructorArguments
                        });
        }

        /// <summary>
        /// Determines whether the specified pair is null.
        /// </summary>
        /// <param name="pair">The pair.</param>
        /// <returns>
        ///   <c>true</c> if [is pair values null] [the specified pair]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPairValuesNull(KeyValuePair<Type, Type> pair)
        {
            return pair.Key == null || pair.Value == null;
        }

        /// <summary>
        /// Resolves the constructor parameters.
        /// </summary>
        /// <param name="constructorInfo">The constructor info.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>True, if resolving constructor parameters succeed.</returns>
        private bool ResolveConstructorParameters(MethodBase constructorInfo, ICollection<object> arguments)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            arguments.Clear();
            var argumentTypes = constructorInfo.GetParameters().OrderBy(x => x.Position).Select(x => x.ParameterType);

            try
            {
                foreach (var parameterInstance in argumentTypes.Select(this.Resolve))
                {
                    arguments.Add(parameterInstance);
                }

                return true;
            }
            catch (NotAssignableException)
            {
                arguments.Clear();
                return false;
            }
        }
    }
}
