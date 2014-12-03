// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainerType.cs" company="pwasiewicz">
//   pwasiewicz
// </copyright>
// <summary>
//   Defines the ContainerType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac.Attributes
{
    using System;

    /// <summary>The container type attribute.</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ContainerType : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ContainerType"/> class.</summary>
        /// <param name="registerAs">The register as type.</param>
        public ContainerType(Type registerAs = null)
        {
            this.As = registerAs;
        }

        /// <summary>Gets the as type.</summary>
        public Type As { get; private set; }
    }
}
