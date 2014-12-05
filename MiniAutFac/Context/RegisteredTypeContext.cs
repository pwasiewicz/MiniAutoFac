namespace MiniAutFac.Context
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using MiniAutFac.Parameters;

    internal class RegisteredTypeContext : IEnumerable<Type>
    {
        private readonly IEnumerable<Type> types;

        public RegisteredTypeContext(IList<Type> types)
        {
            this.types = types;

            this.Parameters = new Dictionary<Type, HashSet<Parameter>>();

            foreach (var type in types)
            {
                this.Parameters.Add(type, new HashSet<Parameter>());
            }
        }

        public Dictionary<Type, HashSet<Parameter>> Parameters;

        public IEnumerator<Type> GetEnumerator()
        {
            return this.types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
