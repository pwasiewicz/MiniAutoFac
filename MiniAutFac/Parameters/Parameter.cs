namespace MiniAutFac.Parameters
{
    using System.Reflection;

    public abstract class Parameter
    {
        public abstract bool IsApplicable(ParameterInfo pi);

        public abstract object GetValue();
    }
}
