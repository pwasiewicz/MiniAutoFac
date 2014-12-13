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
    using MiniAutFac.Context;
    using MiniAutFac.Exceptions;
    using MiniAutFac.Interfaces;
    using MiniAutFac.Resolvers;
    using Scopes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The default container - instance factory.
    /// </summary>
    internal class Container : LifetimeScope
    {
        private readonly IList<ConcreteResolverBase> additionalResolvers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        public Container()
            : base(null)
        {
            this.ResolveImplicit = false;
            this.additionalResolvers = new List<ConcreteResolverBase>();
        }

        /// <summary>
        /// Gets or sets the type container.
        /// </summary>
        internal IDictionary<Type, RegisteredTypeContext> TypeContainer { get; set; }

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
        /// Resolves the instance of type T.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="requestingType">Type of the requesting.</param>
        /// <returns>
        /// New instance of type T.
        /// </returns>
        /// <exception cref="TypeRepositoryEmptyException"></exception>
        /// <exception cref="CannotResolveTypeException">
        /// </exception>
        public object ResolveInternal(Type type, LifetimeScope scope, Type requestingType = null)
        {
            if (this.TypeContainer == null)
            {
                throw new TypeRepositoryEmptyException();
            }

            var desiredType = type;
            foreach (
                var instance in
                    this.additionalResolvers.Where(additionalResolver => additionalResolver.Resolvable(type, scope))
                        .Select(additionalResolver => additionalResolver.Resolve(desiredType, scope)))
            {
                scope.ScopeAllInstances.Add(instance);
                return instance;
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

            if (registeredInstancesPair.Value.OwnFactories.ContainsKey(outputType))
            {
                return registeredInstancesPair.Value.OwnFactories[outputType](new ActivationContext
                                                                              {
                                                                                  ActivatedType = outputType,
                                                                                  CurrentLifetimeScope = scope,
                                                                                  RequestingType = requestingType
                                                                              });
            }

            if (!desiredType.IsAssignableFrom(outputType))
            {
                throw new CannotResolveTypeException();
            }

            var resolvedInstance = this.CreateInstanceRecursive(registeredInstancesPair.Value, outputType);
            scope.ScopeAllInstances.Add(resolvedInstance);
            return resolvedInstance;
        }

        /// <summary>
        /// Creates the instance recursive.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        /// <exception cref="NotAssignableException"></exception>
        internal object CreateInstanceRecursive(RegisteredTypeContext ctx, Type target)
        {
            LinkedList<object> constructorArguments = null;

            var constructors = target.GetConstructors();
            ConstructorInfo ctor = null;
            if (constructors.Any())
            {
                foreach (var constructorInfo in constructors)
                {
                    var parametersInsance = new LinkedList<object>();
                    if (!this.ResolveConstructorParameters(ctx, constructorInfo, parametersInsance))
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

        /// <summary>
        /// Registers the resolver.
        /// </summary>
        /// <param name="additionalResolvableFactory">The additional resolvable factory.</param>
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
        /// <param name="ctx"></param>
        /// <param name="constructorInfo">The constructor ctx.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>True, if resolving constructor parameters succeed.</returns>
        private bool ResolveConstructorParameters(RegisteredTypeContext ctx, MethodBase constructorInfo, ICollection<object> arguments)
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
            var outputType = constructorInfo.DeclaringType;

            var parameters = constructorInfo.GetParameters().OrderBy(x => x.Position);
            var declaredParameters = ctx.Parameters.ContainsKey(outputType) ? ctx.Parameters[outputType] : null;

            try
            {
                foreach (var parameterInfo in parameters)
                {
                    var paramterResolved = false;

                    if (declaredParameters != null)
                    {
                    foreach (
                            var parameterCtx in
                                declaredParameters.Where(parameterCtx => parameterCtx.IsApplicable(parameterInfo)))
                    {
                        arguments.Add(parameterCtx.GetValue(constructorInfo.DeclaringType));
                        paramterResolved = true;
                        break;
                    }
                    }

                    if (paramterResolved)
                    {
                        continue;
                    }

                    var parameterInstance = this.Resolve(parameterInfo.ParameterType, outputType);
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
