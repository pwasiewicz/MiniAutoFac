namespace MiniAutFac.Modules
{
    using System;
    using System.Collections.Generic;

    public abstract class Module
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Module"/> is registered.
        /// </summary>
        internal bool Registered { get; set; }

        /// <summary>
        /// Gets or sets the registered items.
        /// </summary>
        internal List<ItemRegistrationBase> RegisteredItems { get; set; }

        protected Module()
        {
            this.Registered = false;
            this.RegisteredItems = new List<ItemRegistrationBase>();
        }

        /// <summary>
        /// Performs registraion of speicifed types inside module.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public virtual void Registration(ContainerBuilder builder)
        {
        }

        /// <summary>
        /// Invoked, when any instance of registered type within current module is created.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        public virtual void RegisteredInstanceActivated(Type type, object instance)
        {
        }

        /// <summary>
        /// Invoked, when any instance is created.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        public virtual void InstanceActivated(Type type, object instance)
        {
        }

        /// <summary>
        /// Invoked, when any instance of registered type within current module is resolved.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instnace">The instnace.</param>
        public virtual void RegisteredInstanceResolved(Type type, object instnace)
        {
        }

        /// <summary>
        /// Invoked, when any instance is resolved.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instnace">The instnace.</param>
        public virtual void InstanceResolved(Type type, object instnace)
        {
        }
    }
}
