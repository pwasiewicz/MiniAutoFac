namespace MiniAutFac.Scopes.Wrappers
{
    using System;
    using System.Collections.Generic;
    using MiniAutFac.Context;
    using MiniAutFac.Modules;

    internal class ModuleDecorator : Scope
    {
        private readonly Scope scope;

        private readonly RegisteredTypeContext typeContext;

        private readonly IEnumerable<Module> allModules;

        public ModuleDecorator(Scope scope, RegisteredTypeContext typeContext, IEnumerable<Module> allModules)
        {
            this.typeContext = typeContext;
            this.allModules = allModules;
            this.scope = scope;
        }

        public override void GetInstance(LifetimeScope lifetimeScope, Func<object> valueFactory, Type valueType, out object value)
        {
            object requestedInstance;
            this.scope.GetInstance(lifetimeScope, valueFactory, valueType, out requestedInstance);

            if (typeContext.Modules.ContainsKey(valueType))
            {
                typeContext.Modules[valueType].RegisteredInstanceResolved(valueType, requestedInstance);
            }

            foreach (var module in this.allModules)
            {
                module.InstanceResolved(valueType, requestedInstance);
            }

            value = requestedInstance;
        }
    }
}
