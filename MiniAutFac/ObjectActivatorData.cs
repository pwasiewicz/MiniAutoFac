// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectActivatorData.cs" company="Wąsiewicz">
//   Patryk Wąsiewicz
// </copyright>
// <summary>
//   Defines the ObjectActivatorData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MiniAutFac
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    using MiniAutFac.Interfaces;

    /// <summary>
    /// The object activator data.
    /// </summary>
    internal class ObjectActivatorData : IObjectActivatorData
    {
        private IEnumerable<object> constructorArguments;

        /// <summary>
        /// Gets or sets the resolved type.
        /// </summary>
        public Type ResolvedType { get; set; }

        /// <summary>
        /// Gets or sets the constructor info.
        /// </summary>
        public ConstructorInfo ConstructorInfo { get; set; }

        /// <summary>
        /// Gets or sets the constructor arguments.
        /// </summary>
        public IEnumerable<object> ConstructorArguments
        {
            get { return this.constructorArguments; }
            set { this.constructorArguments = new ReadOnlyCollection<object>(value as List<object> ?? value.ToList()); }
        }

        public static ObjectActivatorData ForDefaultConstructor(Type t)
        {
            return new ObjectActivatorData
            {
                ResolvedType = t,
                ConstructorInfo = t.GetConstructor(new Type[0]),
                ConstructorArguments = new object[0]
            };
        }
    }
}
