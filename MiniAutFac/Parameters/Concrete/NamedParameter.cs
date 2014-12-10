namespace MiniAutFac.Parameters.Concrete
{
    using System;
    using System.Reflection;

    public class NamedParameter : Parameter
    {
        private readonly string name;

        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public NamedParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Determines whether the specified pi is applicable.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public override bool IsApplicable(ParameterInfo pi)
        {
            return pi.Name == name;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="resolvedType">Type of the resolved.</param>
        /// <returns></returns>
        public override object GetValue(Type resolvedType)
        {
            return this.value;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(NamedParameter other)
        {
            return string.Equals(this.name, other.name);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NamedParameter)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (this.name != null ? this.name.GetHashCode() : 0);
        }
    }
}
