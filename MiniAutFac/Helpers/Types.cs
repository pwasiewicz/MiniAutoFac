namespace MiniAutFac.Helpers
{
    using System;
    using System.Linq;
    using Interfaces;

    internal static class Types
    {
        private static readonly Type[] ForbiddenTypes =
        {
            typeof (Container),
            typeof (ILifetimeScope),
            typeof (IResolvable),
        };

        internal static bool IsRegistrationForbiddenType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return ForbiddenTypes.Contains(type);
        }
    }
}
