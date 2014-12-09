// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainerBuilder.cs" company="pat.wasiewicz">
//   pat.wasiewicz
// </copyright>
// <summary>
//   The container builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac
{
    using System.Reflection;
    using MiniAutFac.Attributes;
    using MiniAutFac.Context;
    using MiniAutFac.Exceptions;
    using MiniAutFac.Helpers;
    using MiniAutFac.Interfaces;
    using MiniAutFac.Resolvable;
    using MiniAutFac.Resolvers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The container builder.
    /// </summary>
    public class ContainerBuilder
    {
        /// <summary>
        /// The type container.
        /// </summary>
        private readonly List<BuilderResolvableItem> typeContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerBuilder" /> class.
        /// </summary>
        public ContainerBuilder()
        {
            this.typeContainer = new List<BuilderResolvableItem>();
            this.ResolveImplicit = false;
        }

        /// <summary>
        /// Gets or sets the activator engine.
        /// </summary>
        public Func<IObjectActivatorData, object> ActivatorEngine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether types should be resolved implicit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if resolve implicit; otherwise, <c>false</c>.
        /// </value>
        public bool ResolveImplicit { get; set; }

        /// <summary>
        /// Registers the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public BuilderResolvableItemBase Register(Type type)
        {
            var builderItem = new BuilderResolvableItem(this, type);
            this.typeContainer.Add(builderItem);
            return builderItem;
        }

        /// <summary>
        /// Registers the specified type.
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <returns>The IBuilderResolvableItem instance.</returns>
        public BuilderResolvableItemBase Register<T>()
        {
            return this.Register(typeof(T));
        }

        /// <summary>
        /// Builds this instance as IResolvable and check if type collection is correct.
        /// </summary>
        /// <returns>IResolvable instance</returns>
        public ILifetimeScope Build()
        {
            var resolvable = new Container
                                 {
                                     TypeContainer = new Dictionary<Type, RegisteredTypeContext>(),
                                     ResolveImplicit = this.ResolveImplicit
                                 };

            resolvable.RegisterResolver(cnt => new EnumerableResolver());

            if (this.ActivatorEngine == null)
            {
                resolvable.ActivationEngine =
                    data => Activator.CreateInstance(data.ResolvedType, data.ConstructorArguments.ToArray());
            }
            else
            {
                resolvable.ActivationEngine = this.ActivatorEngine;
            }

            foreach (
                var builderResolvableItems in
                    this.typeContainer
                        .Where(resolvableItem => resolvableItem.AsType != null)
                        .GroupBy(resolvableItem => resolvableItem.AsType))
            {
                if (builderResolvableItems.Any(item => item.InTypes.Any(t => t.IsInterface || t.IsAbstract)))
                {
                    throw new NotAssignableException();
                }

                var ctx =
                    new RegisteredTypeContext(
                        builderResolvableItems.Select(item => item.InTypes).SelectMany(types => types).ToList());
                if ((from builderResolvableItem in builderResolvableItems where builderResolvableItem.Parameters.Any() from type in builderResolvableItem.InTypes from param in builderResolvableItem.Parameters where !ctx.Parameters[type].Add(param) select type).Any())
                {
                    throw new InvalidOperationException("Cannot add parameter. The same already registered.");
                }

                foreach (var item in builderResolvableItems)
                {
                    foreach (var inType in item.InTypes)
                    {
                        // TODO - key exist
                        ctx.Scopes.Add(inType, item.Scope);   
                    }
                }


                var pair = new KeyValuePair<Type, RegisteredTypeContext>(builderResolvableItems.Key, ctx);


                resolvable.TypeContainer.Add(pair);
            }

            ResolveDependencies(resolvable);

            return resolvable;
        }

        /// <summary>
        /// Registers the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void Register(params Assembly[] assemblies)
        {
            var allTypes = assemblies.SelectMany(assembly => assembly.GetTypes());
            foreach (var type in allTypes)
            {
                var attributes = GetContainerTypeAttribute(type);
                if (attributes == null)
                {
                    continue;
                }

                foreach (var containerType in attributes)
                {
                    var builderItem = new BuilderResolvableItem(this, type);
                    if (containerType.As != null)
                    {
                        builderItem.AsType = containerType.As;
                    }

                    this.typeContainer.Add(builderItem);
                }
            }
        }


        /// <summary>
        /// Register all types from calling assembly decorated with attribute ContainerType.
        /// </summary>
        public void Register()
        {
            this.Register(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// The get container type attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The <see cref="IEnumerable" />.
        /// </returns>
        private static IEnumerable<ContainerType> GetContainerTypeAttribute(MemberInfo type)
        {
            var attributes = type.GetCustomAttributes(typeof(ContainerType), false).Cast<ContainerType>().ToList();
            return !attributes.Any() ? null : attributes;
        }

        /// <summary>
        /// Resolves dependencies.
        /// </summary>
        /// <param name="resolvable">The instance factory.</param>
        private static void ResolveDependencies(Container resolvable)
        {
            var graph = new Graph<Type>();

            foreach (var type in resolvable.TypeContainer.Values.SelectMany(types =>
                                                                            {
                                                                                var enumerable = types as IList<Type> ??
                                                                                                 types.ToList();
                                                                                return enumerable;
                                                                            }))
            {
                var constructors = type.GetConstructors();
                foreach (
                    var parameterInfo in
                        constructors.Select(constructorInfo => constructorInfo.GetParameters())
                                    .SelectMany(parameteres => parameteres))
                {
                    graph.AddEdge(type, parameterInfo.ParameterType);
                }
            }

            if (graph.HasCycle())
            {
                throw new CircularDependenciesException();
            }
        }
    }
}
