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
    using Extensions;
    using MiniAutFac.Context;
    using MiniAutFac.Exceptions;
    using MiniAutFac.Interfaces;
    using MiniAutFac.Resolvers;
    using Parameters;
    using Scopes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Scopes.Wrappers;
    using Module = Modules.Module;

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
        /// Gets or sets all modules.
        /// </summary>
        internal IEnumerable<Module> AllModules { get; set; }

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
        /// <param name="lifetimeScope">The liftime scope.</param>
        /// <param name="requestingType">Type of the requesting.</param>
        /// <param name="additionalParameters">Additional parameters to use for resolving</param>
        /// <returns>
        /// New instance of type T.
        /// </returns>
        /// <exception cref="TypeRepositoryEmptyException"></exception>
        /// <exception cref="CannotResolveTypeException">
        /// </exception>
        public object ResolveInternal(Type type, LifetimeScope lifetimeScope, Type requestingType = null, object key = null, params Parameter[] additionalParameters)
        {
            if (this.TypeContainer == null)
            {
                throw new TypeRepositoryEmptyException();
            }

            var desiredType = type;
            foreach (
                var instance in
                    this.additionalResolvers.Where(additionalResolver => additionalResolver.Resolvable(type, lifetimeScope))
                        .Select(additionalResolver => additionalResolver.Resolve(desiredType, lifetimeScope, key)))
            {
                lifetimeScope.ScopeAllInstances.Add(instance);
                return instance;
            }

            var registeredInstancesPair = this.TypeContainer.FirstOrDefault(pair => pair.Key == desiredType);
            if (IsPairValuesNull(registeredInstancesPair))
            {
                if (type.IsGenericType)
                {
                    var gn = type.GetGenericTypeDefinition();
                    registeredInstancesPair = this.TypeContainer.FirstOrDefault(pair => pair.Key == gn);

                }

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
            }

            var types = registeredInstancesPair.Value.GetForKey(key).ToList();
            if (types.Skip(1).Any())
            {
                throw new CannotResolveTypeException();
            }

            var outputType = types.First();
            if (outputType.IsGenericType && desiredType.IsGenericType)
            {
                outputType = outputType.MakeGenericType(desiredType.GetGenericArguments());
            }

            if (!(outputType == typeof(object)
                && registeredInstancesPair.Value.OwnFactories.ContainsKey(outputType)))
            {
                if (!desiredType.IsAssignableFrom(outputType))
                {
                    throw new CannotResolveTypeException();
                }
            }

            return this.CreateInstanceRecursive(lifetimeScope, registeredInstancesPair.Value, outputType, desiredType,
                                                requestingType, additionalParameters);
        }

        /// <summary>
        /// Creates the instance recursive.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="target">The target.</param>
        /// <param name="requestingType">Type of the requesting.</param>
        /// <param name="additionalParameters">Additional parameters to use for resolving</param>
        /// <returns></returns>
        /// <exception cref="NotAssignableException"></exception>
        internal object CreateInstanceRecursive(LifetimeScope scope, RegisteredTypeContext ctx, Type target, Type resolvedType, Type requestingType = null, params Parameter[] additionalParameters)
        {
            if (ctx.OwnFactories.ContainsKey(target))
            {
                var inst =
                    ctx.OwnFactories[target](new ActivationContext
                                                {
                                                    ActivatedType = target,
                                                    CurrentLifetimeScope = scope,
                                                    RequestingType = requestingType
                                                });

                if (inst == null) throw new CannotResolveTypeException();
                if (!resolvedType.IsAssignableFrom(inst.GetType())) throw new CannotResolveTypeException();

                return inst;
            }

            if (target == typeof(object)) throw new InvalidOperationException("Object type must have instance factory specified.");

            LinkedList<object> constructorArguments = null;

            var constructors = target.GetConstructors();
            ConstructorInfo ctor = null;
            if (constructors.Any())
            {
                foreach (var constructorInfo in constructors)
                {
                    var parametersInsance = new LinkedList<object>();
                    if (!this.ResolveConstructorParameters(scope, ctx, constructorInfo, parametersInsance, additionalParameters))
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

            var instance =
                this.ActivationEngine(
                    new ObjectActivatorData
                    {
                        ResolvedType = target,
                        ConstructorInfo = ctor,
                        ConstructorArguments = constructorArguments
                    });

            this.ActivateInModules(ctx, target, instance);

            scope.ScopeAllInstances.Add(instance);

            return instance;
        }

        private void ActivateInModules(RegisteredTypeContext ctx, Type target, object instance)
        {
            var moduleForType = ctx.Modules.ContainsKey(target) ? ctx.Modules[target] : null;

            foreach (var module in this.AllModules)
            {
                module.InstanceActivated(target, instance);
                if (moduleForType != null && moduleForType == module)
                {
                    module.RegisteredInstanceActivated(target, instance);
                }
            }
        }

        /// <summary>
        /// Registers the resolver.
        /// </summary>
        /// <param name="additionalResolvableFactory">The additional resolvable factory.</param>
        internal void RegisterResolver(Func<Container, ConcreteResolverBase> additionalResolvableFactory)
        {
            this.additionalResolvers.Add(additionalResolvableFactory(this));
        }

        internal Scope WrapScope(LifetimeScope lifetimeScope, RegisteredTypeContext ctx, Scope desiredScope)
        {
            return new ModuleDecorator(desiredScope, ctx, this.AllModules);
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
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="constructorInfo">The constructor ctx.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="additionalParameters"></param>
        /// <returns>
        /// True, if resolving constructor parameters succeed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// constructorInfo
        /// or
        /// arguments
        /// </exception>
        private bool ResolveConstructorParameters(LifetimeScope lifetimeScope, RegisteredTypeContext ctx, MethodBase constructorInfo, ICollection<object> arguments, Parameter[] additionalParameters)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (additionalParameters == null) additionalParameters = new Parameter[0];

            arguments.Clear();
            var outputType = constructorInfo.DeclaringType;

            var parameters = constructorInfo.GetParameters().OrderBy(x => x.Position);
            var declaredParameters =
                additionalParameters.Concat(ctx.Parameters.ContainsKey(outputType)
                                                ? ctx.Parameters[outputType]
                                                : Enumerable.Empty<Parameter>());

            try
            {
                foreach (var parameterInfo in parameters)
                {
                    var paramterResolved = false;

                    foreach (var parameterCtx in
                                         declaredParameters.Where(parameterCtx => parameterCtx.IsApplicable(parameterInfo)))
                    {
                        arguments.Add(parameterCtx.GetValue(constructorInfo.DeclaringType));
                        paramterResolved = true;
                        break;
                    }

                    if (paramterResolved)
                    {
                        continue;
                    }

                    var parameterInstance = this.Resolve(lifetimeScope, parameterInfo.ParameterType, outputType);
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
