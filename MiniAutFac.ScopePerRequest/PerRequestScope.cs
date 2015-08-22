namespace MiniAutFac.ScopePerRequest
{
    using Exceptions;
    using Scopes;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public static class PerReqestScopeHelper
    {
        public static ItemRegistrationBase PerRequest(this ItemRegistrationBase resolvable)
        {
            return resolvable.WithScope(new PerRequestScope());
        }
    }

    public class PerRequestScope : Scope
    {
        private readonly IDictionary<HttpContext,Dictionary<Type, object>> requestMap;

        public PerRequestScope()
        {
            this.requestMap = new Dictionary<HttpContext, Dictionary<Type, object>>();
        }

        public override void GetInstance(LifetimeScope lifetimeScope, Func<object> valueFactory, Type valueType,
                                         out object value)
        {
            var request = HttpContext.Current;
            if (request == null) throw new CannotResolveTypeException();

            lock (request)
            {
                if (!this.requestMap.ContainsKey(request))
                {
                    this.requestMap.Add(request, new Dictionary<Type, object>());
                    request.AddOnRequestCompleted(ctx => this.requestMap.Remove(ctx));
                }

                Dictionary<Type, object> scopeTypes;

                if (!this.requestMap.TryGetValue(request, out scopeTypes))
                {
                    scopeTypes = new Dictionary<Type, object>();
                    this.requestMap.Add(request, scopeTypes);
                }

                if (!scopeTypes.TryGetValue(valueType, out value))
                {
                    value = valueFactory();
                    scopeTypes.Add(valueType, value);
                }
            }
        }
    }
}
