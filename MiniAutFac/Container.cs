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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using MiniAutFac.Exceptions;
    using MiniAutFac.Interfaces;
    using MiniAutFac.Resolvers;

    /// <summary>
    /// The default container - instance factory.
    /// </summary>
    internal class Container : IResolvable
    {
        private readonly IList<ConcreteResolverBase> additionalResolvers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        public Container()
        {
            this.ResolveImplicit = false;
            this.additionalResolvers = new List<ConcreteResolverBase>();
        }

        /// <summary>
        /// Gets or sets the type container.
        /// </summary>
        internal IDictionary<Type, IEnumerable<Type>> TypeContainer { get; set; }

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
            var resolved = this.Resolve(typeof(T));
            return (T)resolved;
        }

        /// <summary>
        /// Resolves the instance of type T.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns> New instance of type T. </returns>
        public object Resolve(Type type)
        {
            if (this.TypeContainer == null)
            {
                throw new TypeRepositoryEmptyException();
            }

            var desiredType = type;
            foreach (
                var additionalResolver in
                    this.additionalResolvers.Where(additionalResolver => additionalResolver.Resolvable(type)))
            {
                return additionalResolver.Resolve(desiredType);
            }


            var registeredInstancesPair = this.TypeContainer.FirstOrDefault(pair => pair.Key == desiredType);
            if (IsPairValuesNull(registeredInstancesPair))
            {
                if (!this.ResolveImplicit)
                {
                    throw new CannotResolveTypeException();
                }

                registeredInstancesPair =
                    this.TypeContainer.FirstOrDefault(
                        pair => pair.Value.Any(desiredType.IsAssignableFrom));
                if (IsPairValuesNull(registeredInstancesPair))
                {
                    throw new CannotResolveTypeException();
                }
            }

            if (registeredInstancesPair.Value.Skip(1).Any())
            {
                throw new CannotResolveTypeException();
            }

            var outputType = registeredInstancesPair.Value.First();
            if (!desiredType.IsAssignableFrom(outputType))
            {
                throw new CannotResolveTypeException();
            }

            return this.CreateInstanceRecursive(outputType);
        }

        internal object CreateInstanceRecursive(Type target)
        {
            LinkedList<object> constructorArguments = null;

            var constructors = target.GetConstructors();
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
                        ResolvedType = target,
                        ConstructorInfo = ctor,
                        ConstructorArguments = constructorArguments
                    });
        }

        internal void RegisterResolver(Func<Container, ConcreteResolverBase> additionalResolvableFactory)
        {
            this.additionalResolvers.Add(additionalResolvableFactory(this));
        }

        /// <summary>
        /// Determines whether the specified pair is null.
        /// </summary>
        /// <param name="pair">The pair.</param>
        /// <returns>
        ///   <c>true</c> if [is pair values null] [the specified pair]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPairValuesNull<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
            where TKey : class
            where TValue : class
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
