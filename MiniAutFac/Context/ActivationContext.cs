namespace MiniAutFac.Context
{
    using System;
    using MiniAutFac.Interfaces;

    public class ActivationContext
    {
        /// <summary>
        /// Gets or sets the type of the activated.
        /// </summary>
        public Type ActivatedType { get; set; }

        /// <summary>
        /// Gets or sets the type of the requesting type.
        /// </summary>
        public Type RequestingType { get; set; }

        /// <summary>
        /// Gets or sets the current lifetime scope.
        /// </summary>
        public ILifetimeScope CurrentLifetimeScope { get; set; }
    }
}
