namespace MiniAutFac.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Context;

    internal static class RegisteredTypeContextExtensions
    {
        public static IEnumerable<Type> GetForKey(this RegisteredTypeContext ctx, object key = null)
        {
            if (key == null) return ctx.Where(t => !ctx.Keys.ContainsKey(t));

            return ctx.Where(t => ctx.Keys.ContainsKey(t) && ReferenceEquals(ctx.Keys[t], key));
        }
    }
}
