namespace MiniAutFac.Context
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using MiniAutFac.Parameters;
    using Scopes;

    internal class RegisteredTypeContext : IEnumerable<Type>
    {
        private readonly IEnumerable<Type> types;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredTypeContext"/> class.
        /// </summary>
        /// <param name="types">The types.</param>
        public RegisteredTypeContext(IList<Type> types)
        {
            this.types = types;

            this.Parameters = new Dictionary<Type, HashSet<Parameter>>();
            this.Scopes = new Dictionary<Type, Scope>();

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
