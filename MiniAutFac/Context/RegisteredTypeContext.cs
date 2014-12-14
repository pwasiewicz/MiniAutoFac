namespace MiniAutFac.Context
{
    using MiniAutFac.Parameters;
    using Modules;
    using Scopes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class RegisteredTypeContext : IEnumerable<Type>
    {
        /// <summary>
        /// The types
        /// </summary>
        private readonly HashSet<Type> types;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredTypeContext"/> class.
        /// </summary>
        /// <param name="types">The types.</param>
        public RegisteredTypeContext(IList<Type> types)
        {
            this.types = new HashSet<Type>();
            foreach (var type in types.Where(type => !this.types.Add(type)))
            {
                throw new InvalidOperationException("Double type registration. Type: " + type.FullName);
            }

            this.Parameters = new Dictionary<Type, HashSet<Parameter>>();
            this.OwnFactories = new Dictionary<Type, Func<ActivationContext, object>>();
            this.Scopes = new Dictionary<Type, Scope>();
            this.Modules = new Dictionary<Type, Module>();

            foreach (var type in types)
            {
                this.Parameters.Add(type, new HashSet<Parameter>());
            }
        }

        /// <summary>
        /// The parameters
        /// </summary>
        public Dictionary<Type, HashSet<Parameter>> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public Dictionary<Type, Scope> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the modules.
        /// </summary>
        public Dictionary<Type, Module> Modules { get; set; }

        /// <summary>
        /// Gets or sets the own factories.
        /// </summary>
        public Dictionary<Type, Func<ActivationContext, object>> OwnFactories { get; set; } 

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Type> GetEnumerator()
        {
            return this.types.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
