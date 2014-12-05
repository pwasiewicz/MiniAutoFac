namespace MiniAutFac.Parameters.Concrete
{
    using System.Reflection;

    public class NamedParameter : Parameter
    {
        private readonly string name;

        private readonly object value;

        public NamedParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public override bool IsApplicable(ParameterInfo pi)
        {
            return pi.Name == name;
        }

        public override object GetValue()
        {
            return this.value;
        }

        protected bool Equals(NamedParameter other)
        {
            return string.Equals(this.name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NamedParameter) obj);
        }

        public override int GetHashCode()
        {
            return (this.name != null ? this.name.GetHashCode() : 0);
        }
    }
}
