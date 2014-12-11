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
    using MiniAutFac.Attributes;
    using MiniAutFac.Context;
    using MiniAutFac.Exceptions;
    using MiniAutFac.Helpers;
    using MiniAutFac.Interfaces;
    using MiniAutFac.Parameters;
    using MiniAutFac.Resolvable;
    using MiniAutFac.Resolvers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Module = Modules.Module;

    /// <summary>
    /// The container builder.
    /// </summary>
    public class ContainerBuilder
    {
        /// <summary>
        /// The type container.
        /// </summary>
        private readonly List<ItemRegistration> typeContainer;

        /// <summary>
        /// The modules
        /// </summary>
        private readonly HashSet<Module> modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerBuilder" /> class.
        /// </summary>
        public ContainerBuilder()
        {
            this.typeContainer = new List<ItemRegistration>();
            this.modules = new HashSet<Module>();
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

        #region Registering

        /// <summary>
        /// Registers the speicifed module.
        /// </summary>
        /// <param name="module">The module instance to register.</param>
        /// <exception cref="System.ArgumentNullException">module</exception>
        public void RegisterModule(Module module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }

            this.modules.Add(module);
        }

        /// <summary>
        /// Registers the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Instance of ItemRegistrationBase that allows to specify additional configuration.</returns>
        public ItemRegistrationBase Register(Type type)
        {
            var builderItem = new ItemRegistration(this, type);
            this.typeContainer.Add(builderItem);
            return builderItem;
        }

        /// <summary>
        /// Registers the specified type.
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <returns>The IBuilderResolvableItem instance.</returns>
        public ItemRegistrationBase Register<T>()
        {
            return this.Register(typeof(T));
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
                    var builderItem = new ItemRegistration(this, type);
                    if (containerType.As != null)
                    {
                        builderItem.AsType = containerType.As;
                    }

                    this.typeContainer.Add(builderItem);
                }
            }
        }

        /// <summary>
        /// Registers the specified types from assemblies that meets predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>Instance of ItemRegistrationBase that allows to specify additional configuration.</returns>
        public ItemRegistrationBase Register(Predicate<Type> predicate, params Assembly[] assemblies)
        {
            var matchingTypes =
                assemblies.SelectMany(assembly => assembly.GetTypes())
                          .Where(type => !type.IsInterface && !type.IsAbstract)
                          .Where(type => predicate(type));

            var resolvable = new ItemRegistration(this, matchingTypes.ToArray());
            this.typeContainer.Add(resolvable);

            return resolvable;
        }

        /// <summary>
        /// Registers the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>Instance of ItemRegistrationBase that allows to specify additional configuration.</returns>
        public ItemRegistrationBase Register(params Type[] types)
        {
            var resolvable = new ItemRegistration(this, types);
            this.typeContainer.Add(resolvable);

            return resolvable;
        }


        /// <summary>
        /// Register all types from calling assembly decorated with attribute ContainerType.
        /// </summary>
        public void Register()
        {
            this.Register(Assembly.GetCallingAssembly());
        }

        #endregion

        /// <summary>
        /// Builds this instance as IResolvable and check if type collection is correct.
        /// </summary>
        /// <returns>IResolvable instance</returns>
        public ILifetimeScope Build()
        {
            this.ModuleRegistration();

            var resolvable = new Container
                             {
                                 TypeContainer = new Dictionary<Type, RegisteredTypeContext>(),
                                 ResolveImplicit = this.ResolveImplicit,
                                 AllModules = this.modules.ToList()
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


                foreach (var builderResolvableItem in builderResolvableItems)
                {

                    if (builderResolvableItem.OwnFactory != null)
                    {
                        ctx.OwnFactories = builderResolvableItem.InTypes.ToDictionary(type => type,
                                                                                      type =>
                                                                                      builderResolvableItem.OwnFactory);
                    }

                    foreach (var type in builderResolvableItem.InTypes)
                    {
                        foreach (var parameter in builderResolvableItem.Parameters)
                        {
                            if (!ctx.Parameters.ContainsKey(type))
                            {
                                ctx.Parameters.Add(type, new HashSet<Parameter>(new[] { parameter }));
                            }
                            else
                            {
                                if (!ctx.Parameters[type].Add(parameter))
                                {
                                    throw new InvalidOperationException(
                                        "Cannot add parameter. The same already registered.");
                                }
                            }
                        }
                    }
                }

                foreach (var item in builderResolvableItems)
                {
                    foreach (var inType in item.InTypes)
                    {
                        if (item.Module != null)
                        {
                            ctx.Modules.Add(inType, item.Module);
                        }

                        // TODO - key exist
                        ctx.Scopes.Add(inType, item.Scope);
                    }
                }

                var pair = new KeyValuePair<Type, RegisteredTypeContext>(builderResolvableItems.Key, ctx);
                resolvable.TypeContainer.Add(pair);
            }

            CheckForCycles(resolvable);

            GC.Collect();

            return resolvable;
        }

        private void ModuleRegistration()
        {
            this.ModuleRegistration(this.modules);
        }

        /// <summary>
        /// Registers the modules.
        /// </summary>
        /// <param name="modules">The modules.</param>
        private void ModuleRegistration(IEnumerable<Module> modules)
        {
            var childModules = new List<Module>();
            foreach (var module in modules.Where(module => !module.Registered))
            {
                var isolatedBuilder = new ContainerBuilder();

                module.Registration(isolatedBuilder);
                module.Registered = true;

                foreach (var moduleResolvableItem in isolatedBuilder.typeContainer)
                {
                    moduleResolvableItem.Module = module;
                    module.RegisteredItems.Add(moduleResolvableItem);
                    this.typeContainer.Add(moduleResolvableItem);
                }

                if (isolatedBuilder.modules.Any())
                {
                    this.ModuleRegistration(isolatedBuilder.modules);
                }

                childModules.AddRange(isolatedBuilder.modules);
            }

            foreach (var childModule in childModules)
            {
                this.modules.Add(childModule);
            }
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
        private static void CheckForCycles(Container resolvable)
        {
            var graph = new Graph<Type>();

            foreach (var type in resolvable.TypeContainer.Values.SelectMany(types => types.ToList()))
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
