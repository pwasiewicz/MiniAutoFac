namespace MiniAutFac.Modules
{
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
        internal List<BuilderResolvableItemBase> RegisteredItems { get; set; }

        protected Module()
        {
            this.Registered = false;
            this.RegisteredItems = new List<BuilderResolvableItemBase>();
        }

        /// <summary>
        /// Performs registraion of speicifed types inside module.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public virtual void Registration(ContainerBuilder builder)
        {
        }
    }
}
