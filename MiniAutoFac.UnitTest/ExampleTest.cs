// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleTest.cs" company="pat.wasiewicz">
//   Patryk pat.wasiewicz
// </copyright>
// <summary>
//   Defines the ExampleTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutoFac.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniAutFac;
    using MiniAutFac.Exceptions;
    using MiniAutFac.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TestClasses;
    using TestClasses.EnumerableBug;

    /// <summary>
    /// The example test.
    /// </summary>
    [TestClass]
    public class ExampleTest
    {

        [TestMethod]
        public void ResolvingNamedParameters()
        {
            var builder = new ContainerBuilder();
            builder.Register<ParameterClassA>().WithNamedParameter("test", "A").As<ParameterClassA>();
            builder.Register<ParameterClassB>().WithNamedParameter("test", 2).As<ParameterClassB>();

            var cnt = builder.Build();

            var bInst = cnt.Resolve<ParameterClassB>();

            Assert.AreEqual(bInst.Test, 2);
            Assert.AreEqual(bInst.ClassA.Test, "A");
        }

        [TestMethod]
        public void ResolvingEvalutedParameter()
        {
            var builder = new ContainerBuilder();
            builder.Register<ClassThatLogs>()
                   .WithEvalutedParamter("logger", requestingType => new SampleLogger(requestingType));

            var instance = builder.Build().BeginLifetimeScope().Resolve<ClassThatLogs>();

            Assert.AreEqual(typeof(ClassThatLogs), instance.GetLoggedType);
        }

        /// <summary>
        /// The registering without "as".
        /// </summary>
        [TestMethod]
        public void RegisteringWithoutAs()
        {
            var builder = new ContainerBuilder();

            builder.Register<ClassA>();
            var resolver = builder.Build();

            var resolvedInstance = resolver.Resolve<ClassA>();

            Assert.AreEqual(typeof(ClassA), resolvedInstance.GetType());
        }

        /// <summary>
        /// Registering the subclass.
        /// </summary>
        [TestMethod]
        public void RegisteringSubclass()
        {
            var builder = new ContainerBuilder();

            builder.Register<ClassB>().As<ClassA>();
            var resolver = builder.Build();

            var resolvedInstance = resolver.Resolve<ClassA>();

            Assert.AreEqual(typeof(ClassB), resolvedInstance.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DoulbeRegistration()
        {
            var bld = new ContainerBuilder();

            bld.Register<ClassB>().As<ClassA>();
            bld.Register<ClassB>().As<ClassA>();

            bld.Build();
        }

        /// <summary>
        /// Registering the interface.
        /// </summary>
        [TestMethod]
        public void RegisteringInterface()
        {
            var builder = new ContainerBuilder();

            builder.Register<ClassB>().As<InterfaceForClassB>();
            var resolver = builder.Build();

            var resolvedInstance = resolver.Resolve<InterfaceForClassB>();

            Assert.AreEqual(typeof(ClassB), resolvedInstance.GetType());
        }

        /// <summary>
        /// Registering the not assignable class.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotAssignableException))]
        public void RegisteringNotAssignableClass()
        {
            var builder = new ContainerBuilder();

            builder.Register<ClassA>().As<ClassB>();
            builder.Build();
        }

        /// <summary>
        /// Registering the same type twice.
        /// </summary>
        [TestMethod]
        public void RegisteringTheSameTypeTwice()
        {
            var builder = new ContainerBuilder();

            builder.Register<ClassA>().As<IFoo>();
            builder.Register<ClassB>().As<IFoo>();
            var continer = builder.Build();

            var impl = continer.Resolve<IEnumerable<IFoo>>();

            Assert.AreEqual(impl.Count(), 2);
        }

        /// <summary>
        /// Resolving the type of the unregistered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CannotResolveTypeException))]
        public void ResolvingUnregisteredType()
        {
            var builder = new ContainerBuilder();
            builder.Build().Resolve<ClassA>();
        }

        /// <summary>
        /// Registering the interface as in and out types.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotAssignableException))]
        public void RegisteringInterfaceAsInAndOut()
        {
            var builder = new ContainerBuilder();
            builder.Register<InterfaceForClassB>();
            builder.Build().Resolve<InterfaceForClassB>();
        }

        /// <summary>
        /// Resolving the implicit.
        /// </summary>
        [TestMethod]
        public void ResolvingImplicit()
        {
            var builder = new ContainerBuilder { ResolveImplicit = true };
            builder.Register<ClassB>();
            var factory = builder.Build();

            var resolvedInstance = factory.Resolve<InterfaceForClassB>();

            Assert.AreEqual(typeof(ClassB), resolvedInstance.GetType());
        }

        /// <summary>
        /// Resolving the implicit not registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CannotResolveTypeException))]
        public void ResolvingImplicitNotRegistered()
        {
            var builder = new ContainerBuilder { ResolveImplicit = true };
            builder.Register<ClassA>();
            var factory = builder.Build();

            var resolvedInstance = factory.Resolve<InterfaceForClassB>();

            Assert.AreEqual(typeof(ClassB), resolvedInstance.GetType());
        }

        /// <summary>
        /// Resolving types with dependencies.
        /// </summary>
        [TestMethod]
        public void ResolvingSimpleDependency()
        {
            var builder = new ContainerBuilder { ResolveImplicit = true };
            builder.Register<ClassB>().As<InterfaceForClassB>();
            builder.Register<ClassV>();
            var factory = builder.Build();

            var resolvedInstance = factory.Resolve<ClassV>();

            Assert.AreEqual(typeof(ClassV), resolvedInstance.GetType());
        }

        /// <summary>
        /// Resolving circular dependencies.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularDependenciesException))]
        public void ResolvingCircular()
        {
            var builder = new ContainerBuilder { ResolveImplicit = true };
            builder.Register<Class1>();
            builder.Register<Class2>();
            var factory = builder.Build();

            var resolvedInstance = factory.Resolve<Class2>();

            Assert.AreEqual(typeof(Class2), resolvedInstance.GetType());
        }

        [TestMethod]
        public void RegisterTypesCollectionFromAssembly()
        {
            var builder = new ContainerBuilder();
            builder.Register(Assembly.GetExecutingAssembly());

            var instances = builder.Build().Resolve<IEnumerable<ClassA>>();

            Assert.AreEqual(instances.Count(), 2);
        }

        /// <summary>The registering types with attribute.</summary>
        [TestMethod]
        public void RegisteringTypesWithAttribute()
        {
            var builder = new ContainerBuilder();
            builder.Register();
            var factory = builder.Build();

            var resolved1 = factory.Resolve<InterfaceForClassB>();
            var resolved2 = factory.Resolve<IEnumerable<ClassA>>();

            Assert.AreEqual(resolved1.GetType(), typeof(ClassB));
            Assert.AreEqual(resolved2.Count(), 2);
        }

        [TestMethod]
        public void AsImplementedInterfaceRegistration()
        {
            var builder = new ContainerBuilder();
            builder.Register<ClassA>().AsImplementedInterfaces();

            var cnt = builder.Build();
            var instance = cnt.Resolve<IFoo>();

            Assert.AreEqual(typeof(ClassA), instance.GetType());
        }

        [TestMethod]
        public void NestedLifetimeScopeInstancePerDependency()
        {
            var builder = new ContainerBuilder();
            builder.Register<ClassA>().As<IFoo>().PerLifetimeScope();

            var cnt = builder.Build();
            var firstInstance = cnt.Resolve<IFoo>();

            var nestedScope = cnt.BeginLifetimeScope();
            var nestedFirst = nestedScope.Resolve<IFoo>();

            var secondInstance = cnt.Resolve<IFoo>();
            var nestedSecond = nestedScope.Resolve<IFoo>();

            Assert.AreSame(firstInstance, secondInstance);
            Assert.AreSame(nestedFirst, nestedSecond);
            Assert.AreNotSame(firstInstance, nestedFirst);
        }

        [TestMethod]
        public void NestedLifetimeScopSingleeInstance()
        {
            var builder = new ContainerBuilder();
            builder.Register<ClassA>().As<IFoo>().SingleInstance();

            var cnt = builder.Build();
            var firstInstance = cnt.Resolve<IFoo>();

            var nestedScope = cnt.BeginLifetimeScope();
            var nestedFirst = nestedScope.Resolve<IFoo>();

            var secondInstance = cnt.Resolve<IFoo>();
            var nestedSecond = nestedScope.Resolve<IFoo>();

            Assert.AreSame(firstInstance, secondInstance);
            Assert.AreSame(nestedFirst, nestedSecond);
            Assert.AreSame(firstInstance, nestedFirst);
        }

        [TestMethod]
        public void InstancesPerLifeScopeDispoed()
        {
            var bld = new ContainerBuilder();
            bld.Register<DisposableClass>().PerLifetimeScope();

            var cnt = bld.Build();

            var mainInstance = cnt.Resolve<DisposableClass>();

            var nestedScope = cnt.BeginLifetimeScope();
            var nestedInstance = nestedScope.Resolve<DisposableClass>();

            cnt.Dispose();

            Assert.IsTrue(mainInstance.DisposeCalled);
            Assert.IsTrue(nestedInstance.DisposeCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(LifetimeScopeDisposedException))]
        public void ResolvingAfterDisposalThrowsException()
        {
            var bld = new ContainerBuilder();
            bld.Register<DisposableClass>();

            var cnt = bld.Build();
            cnt.Dispose();

            cnt.Resolve<DisposableClass>();
        }

        [TestMethod]
        public void RegisteringTypesWithPredicates()
        {
            var bld = new ContainerBuilder();
            bld.Register(type => typeof(IFoo).IsAssignableFrom(type), Assembly.GetExecutingAssembly()).As<IFoo>();

            var container = bld.Build();

            var foos = container.Resolve<IEnumerable<IFoo>>();

            Assert.AreEqual(3, foos.Count());
        }

        [TestMethod]
        public void ResolvingMultipleTypesWithPredicateAndPerLiftetimeScope()
        {
            var bld = new ContainerBuilder();
            bld.Register(type => typeof(IFoo).IsAssignableFrom(type), Assembly.GetExecutingAssembly())
               .As<IFoo>()
               .PerLifetimeScope();

            var container = bld.Build();

            var foos = container.Resolve<IEnumerable<IFoo>>().ToList();

            Assert.AreEqual(3, foos.Count());
            Assert.AreNotSame(foos[0], foos[1]);
            Assert.AreNotSame(foos[1], foos[2]);
        }

        [TestMethod]
        public void RegisteringConcreteInstance()
        {
            var instanceB = new ClassB();

            var bld = new ContainerBuilder();
            bld.Register<ClassA>().As(instanceB).PerDependency();

            using (var cnt = bld.Build())
            {
                var mainB = cnt.Resolve<ClassA>();
                ClassA nestedB;

                using (var scope = cnt.BeginLifetimeScope())
                {
                    nestedB = scope.Resolve<ClassA>();
                }

                Assert.AreSame(instanceB, mainB);
                Assert.AreSame(instanceB, nestedB);
            }
        }

        [TestMethod]
        public void RegisteringFactoryOfInstance()
        {
            var called = 0;

            var bld = new ContainerBuilder();
            bld.Register<ClassA>().As(ctx =>
                                      {
                                          called += 1;
                                          return new ClassB();
                                      }).PerLifetimeScope();

            using (var cnt = bld.Build())
            {
                cnt.Resolve<ClassA>();
                cnt.Resolve<ClassA>();

                using (var scope = cnt.BeginLifetimeScope())
                {
                    scope.Resolve<ClassA>();
                }
            }

            Assert.AreEqual(2, called);
        }

        [TestMethod]
        public void SomeDisposableWithinhInsideContext()
        {
            var builder = new ContainerBuilder();
            builder.Register<SomeContext>().PerLifetimeScope();
            builder.Register<SomeDisposable>().PerLifetimeScope();

            SomeContext ctx;

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                ctx = scope.Resolve<SomeContext>();
            }

            Assert.IsTrue(ctx.Disposables.First().Disposed);
        }

        [TestMethod]
        public void RegisteringClassesWithModule()
        {
            var bld = new ContainerBuilder();
            bld.RegisterModule(new SampleModule());

            var aInstance = bld.Build().Resolve<ClassA>();

            Assert.AreEqual(typeof(ClassB), aInstance.GetType());
        }

        [TestMethod]
        public void RegisteringEmbeddedhModulse()
        {
            var bld = new ContainerBuilder();
            bld.RegisterModule(new EmbeddingModule());

            var cnt = bld.Build();
            var aInstance = cnt.Resolve<ClassA>();
            var fooInstance = cnt.Resolve<IFoo>();

            Assert.AreEqual(typeof(ClassB), aInstance.GetType());
            Assert.AreEqual(typeof(ClassB), fooInstance.GetType());
        }

        [TestMethod]
        public void ModuleActivationListener()
        {
            var bld = new ContainerBuilder();
            bld.RegisterModule(new ModuleWithRegisteredInstanceListener());
            var cnt = bld.Build();

            var instance = cnt.Resolve<ActivableClass>();

            Assert.IsTrue(instance.Activated);
        }


        [TestMethod]
        public void ModuleResolvingRegisteredCalledProperly()
        {
            var mod = new ModuleWithResolveTrackingAndOwnRegistration();
            var bld = new ContainerBuilder();
            bld.RegisterModule(mod);

            ClassA i1;
            ClassA i2;

            using (var cnt = bld.Build().BeginLifetimeScope())
            {
                i1 = cnt.Resolve<ClassA>();
                i2 = cnt.Resolve<ClassA>();
            }

            Assert.AreSame(i1, i2);
            Assert.AreEqual(2, mod.Called);
        }


        [TestMethod]
        public void ModuleResolvingCalledProperly()
        {
            var mod = new ModuleWithResolveTracking();
            var bld = new ContainerBuilder();
            bld.Register<ClassA>().PerLifetimeScope();
            bld.RegisterModule(mod);

            ClassA i1;
            ClassA i2;

            using (var cnt = bld.Build().BeginLifetimeScope())
            {
                i1 = cnt.Resolve<ClassA>();
                i2 = cnt.Resolve<ClassA>();
            }

            Assert.AreSame(i1, i2);
            Assert.AreEqual(2, mod.Called);
        }


        [TestMethod]
        public void Register_RegisteringFactoryAsType()
        {
            var bld = new ContainerBuilder();
            var bInst = new ClassB();
            bld.Register(context => bInst).As<InterfaceForClassB>();
            var cnt = bld.Build();
            Assert.AreSame(bInst, cnt.Resolve<InterfaceForClassB>());
        }

        [TestMethod]
        public void Resolve_Lazy()
        {
            var bld = new ContainerBuilder();
            bld.Register<ClassInstanceCount>().PerLifetimeScope();
            ClassInstanceCount.Instances = 0;

            var cnt = bld.Build();

            var laztInst = cnt.Resolve<Lazy<ClassInstanceCount>>();

            Assert.AreEqual(0, ClassInstanceCount.Instances);
            // ReSharper disable once NotAccessedVariable
            var inst = laztInst.Value;
            Assert.AreEqual(1, ClassInstanceCount.Instances);

            laztInst = cnt.Resolve<Lazy<ClassInstanceCount>>();
            // ReSharper disable once RedundantAssignment
            inst = laztInst.Value;

            Assert.AreEqual(1, ClassInstanceCount.Instances);
        }

        [TestMethod]
        public void Resolve_LazyCombinedWithEnumerable()
        {
            var bld = new ContainerBuilder();
            bld.Register<ClassInstanceCount>();

            var cnt = bld.Build();
            var instancesEvaluator = cnt.Resolve<Lazy<IEnumerable<ClassInstanceCount>>>();

            var instances = instancesEvaluator.Value;

            Assert.AreEqual(1, instances.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(RegistrationNotAllowedException))]
        public void Register_ForbidenType_ThrowsExcp()
        {

            (new ContainerBuilder()).Register<LifetimeProxy>().As<ILifetimeScope>();
        }

        [TestMethod]
        public void Resolve_ClassWithLifeTimeScopeInjected_ReturnsProperInstances()
        {
            var bld = new ContainerBuilder();
            bld.Register<InjectLifetimeScopeClass>();

            var cnt = bld.Build();

            var cl = cnt.Resolve<InjectLifetimeScopeClass>();

            Assert.AreSame(cnt, cl.Scope);
        }

        [TestMethod]
        public void Resolve_ClassWithLifeTimeScopeInjected_NestedLifetimeScope_ReturnsProperInstances()
        {
            var bld = new ContainerBuilder();
            bld.Register<InjectLifetimeScopeClass>();

            var cnt = bld.Build();

            var lf = cnt.BeginLifetimeScope();
            var cl = lf.Resolve<InjectLifetimeScopeClass>();

            Assert.AreSame(lf, cl.Scope);
        }

        [TestMethod]
        public void Keyed_Resolve()
        {
            const string someKey = "someKey";

            var bld = new ContainerBuilder();
            bld.Register<ClassB>().As<IFoo>().Keyed(someKey);
            bld.Register<ClassA>().As<IFoo>();

            var cnt = bld.Build();

            var firstInstance = cnt.Resolve<IFoo>();
            var second = cnt.ResolveKeyed<IFoo>(someKey);


            Assert.IsInstanceOfType(firstInstance, typeof(ClassA));
            Assert.IsInstanceOfType(second, typeof(ClassB));
        }

        [TestMethod]
        public void Resolve_Keyed_WithIEnumerable()
        {
            const string someKey = "someKey";

            var bld = new ContainerBuilder();
            bld.Register<ClassB>().As<IFoo>().Keyed(someKey);
            bld.Register<ClassA>().As<IFoo>().Keyed(someKey);

            bld.Register<FooClass>().As<IFoo>();

            var cnt = bld.Build();

            var coll = cnt.ResolveKeyed<IEnumerable<IFoo>>(someKey).ToList();
            var inst = cnt.Resolve<IFoo>();

            Assert.IsInstanceOfType(inst, typeof(FooClass));
            Assert.AreEqual(2, coll.Count);
        }

        [TestMethod]
        public void RegisterAndResolve_ValidFactory_GeneralObjectFacotory()
        {
            var target = new ClassA();
            var bld = new ContainerBuilder();
            bld.Register(ctx => (object)target).As<IFoo>();
            var cnt = bld.Build();

            var inst = cnt.Resolve<IFoo>();

            Assert.AreSame(target, inst);
        }

        [TestMethod, ExpectedException(typeof(CannotResolveTypeException))]
        public void RegisterAndResolve_InvalidFactory_ThrowsExcp()
        {
            var target = new ClassA();
            var bld = new ContainerBuilder();
            bld.Register(ctx => (object)target).As<IDisposable>();
            var cnt = bld.Build();

            cnt.Resolve<IDisposable>();
        }
    }
}