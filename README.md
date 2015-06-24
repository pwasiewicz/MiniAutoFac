miniautofac
===========

Simple IoC container for Microsoft .NET

Description
-----------

MiniAutFac was created in order to understand how reflection and original AutoFac work. It is not dedicated for complex projects (see http://code.google.com/p/autofac/ instead) - rather for educational purposes and simple solutions.

Features
* Simple code 
* Lightweight (about 30 kb)
* Resolving constructors parameters
* Detecting circular dependencies (only throws exception)
* Attribute-based registration
* AutoFac-like lifetime scopes

Releases
-----------
### 1.3.5
* Fixed small bug when resolving keyed services with IEnumerable.

### 1.3.4
* Validating type of instance factory result at resolve time (not while registering) when object is used as a return type

### 1.3.3
* Resolving services by a key

### 1.3.2
* Resolving Lazy - instances are resolved only when Value is called.

### 1.3.1
* Registering type via value factory

### 1.3.0
* Parameters (named and evaluted paramteres)
* Registering own instance factory for type
* Simple modules
* Fixed bug: when resolving enumerable within nested liftime scope and resolved instance was disposed incorrectly

### 1.2.2
* Fixed invalid assembly version inside nuget package.

### 1.2.1
* Fixed bug when resolving `IEnumerable` regstired per lifetime scope

### 1.2
* Removed registering from namespace (found it useless and not efficient)
* Registering multiple classes / types chosen with predicate within assemblies
* Registering multiple items

### 1.1
* First release
* Base features


Usage
-----------

### Self-registering
```c#
var builder = new ContainerBuilder();
builder.Register<Foo>();

var resolver = builder.Build();
var fooInstance = resolver.Resolve<Foo>()
```

### Registering class as class
```c#
var builder = new ContainerBuilder();
builder.Register<Bar>().As<Foo>();

var resolver = builder.Build();
var fooInstance = resolver.Resolve<Foo>(); // fooInstance.GetType() == typeof(Bar)
```

### Registering type wth implict registration

```c#
public class Foo: IFoo {
}
...
var builder = new ContainerBuilder() { ResolveImplicit = true };
builder.Register<Foo>();

var resolver = builder.Build();
var fooInstance = resolver.Resolve<IFoo>(); // fooInstance.GetType() == typeof(Foo)
```


### Registering multiple classes and resolving:
```c#
var builder = new ContainerBuilder();
builder.Register<Foo1>().As<IFoo>();
builder.Register<Foo2>().As<IFoo>();

IEnumerable<IFoo> fooInstances = builder.Build().Resolve<IEnumerable<IFoo>>();
```

### Attribute-based registration
```c#
[ContainerType(typeof(Bar))]
[ContainerType(typeof(IFoo))]
class Foo: Bar, IFoo
{
}

...

var builder = new ContainerBuilder();
builder.Register();
var container = builder.Build();

var instance1 = container.Resolve<Bar>();
var instance2 = container.Resolve<IFoo>();
```

### Registering type as implemented interfaces
```c#
var builder = new ContainerBuilder();
builder.Register<Foo>().AsImplementedInterfaces();

var cnt = builder.Build();
var instance = cnt.Resolve<IFoo>();
```

### Registering multiple types with one registration
```c#
var bld = new ContainerBuilder();
bld.Register(type => typeof(IFoo).IsAssignableFrom(type), 
                            Assembly.GetExecutingAssembly()).As<IFoo>();

var container = bld.Build();
var foos = container.Resolve<IEnumerable<IFoo>>();
```

### Lifetime scopes

After building container, you can create many nested lifetime scopes that manages a life cycle of registered instances.

```c#
var builder = new ContainerBuilder();
builder.Register<Foo>().PerLifetimeScope();

using (var container = builder.Build())
{
    var foo = container.Resolve<Foo>();
    
    var scope = container.BeginLifetimeScope();
    var nextFoo = scope.Resolve<Foo>();
    
    var anotherFoo = container.Resolve<Foo>();
    
    // when container is disposed, it disposes also all child scopes
}

// references foo and anotherFoo are the same, but they are different to nextFoo 
//(it was created in other scope).
```

Possible scopes so far:
* `SingleInstance` - single instance per container (shared among all nested lifetime scopes)
* `PerDependency` - instance created every `Resolve` request
* `PerLifetimeScope` - instance shared among single lifetime scope
* 

### Named paramter
You can tells the resolvable to to apply own buult instance as parameter, when resolving:

```c#
public class Foo
{
    public Foo(string parameter1) 
    {
    }
}

var builder = new ContainerBuilder();
builder.Register<Foo>()
       .WithNamedParameter("paramter1", "value");
var cnt = builder.Build();
var fooInst = cnt.Resolve<Foo>();
```

### Evaluated parameter
You can create own instance factory for paramter:

```c#
var builder = new ContainerBuilder();
builder.Register<Foo>().WithEvalutedParameter(
                           "logger", 
                           requesingType => new Logger(requestingType));
var foo = builder.Build().Resolve<Foo>();
```
### Own instnace factory
```c#
bld.Register<Foo>().As(ctx =>
                             {
                                 // some other methods
                                 return new Logger(ctx.RequestingType); 
                                   // Requesting type is type, that request that instance
                                   // when f.e. injected as construcot parameter. Can be null, if type
                                   // resolved directly
                              }).PerLifetimeScope();
```

### Registering type via value factory
```c#
var bld = new ContainerBuilder();
var bInst = new ClassB();
bld.Register(context => bInst).As<InterfaceForClassB>();
```
            
