miniautofac
===========

Simple IoC container for Microsoft .NET

Description
-----------

MiniAutFac was created in order to understand how reflection and original AutoFac work. It is not dedicated for complex projects (see http://code.google.com/p/autofac/ instead) - rather for educational purposes and simple solutions.

Features
* Simple code 
* Resolving constructors parameters
* Detecting circular dependencies (only throws exception)
* Attribute-based registration

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

### Registering instance as all implemented interfaces

```c#
public class Foo: IFoo {
}
...
var builder = new ContainerBuilder() { ResolveImplicit = true };
builder.Register<Foo>().As<IFoo>();

var resolver = builder.Build();
var fooInstance = resolver.Resolve<IFoo>(); // fooInstance.GetType() == typeof(Foo)
```
### Auto-registering types from namespaces

```c#
var builder = new ContainerBuilder { ResolveImplicit = true };
builder.Register("Some.Awsomme.NS");

var classInstance = builder.Build().Resolve<Foo>();
```

### Registering multiple classes and resolving:
```c#
var builder = new ContainerBuilder();
builder.Register<Foo1>().As<IFoo>();
builder.Register<Foo2>().As<IFoo>();

IEnumerable<IFoo> fooInstances = builder.Build().Resolve<IEnumerable<IFoo>();
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

Future
----------

* Resolving collection (injection of `IEnumerable` type like AutoFac
* `IDispolable` support
* Lifetimescope similar to AutoFac
