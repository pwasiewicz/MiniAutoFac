namespace MiniAutFac.ScopePerRequest.Tests
{
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.SessionState;
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestClass;

    [TestClass]
    public class PerRequestScope
    {
        [TestMethod, ExpectedException(typeof(CannotResolveTypeException))]
        public void ResolvingPerRequest_NoRequest_ThrowsException()
        {
            var bld = new ContainerBuilder();
            bld.Register<ClassA>().PerRequest();

            var cnt = bld.Build();
            cnt.Resolve<ClassA>();
        }

        [TestMethod]
        public void Resolving_FakeRequest_ReturnsTheSmaeInstance()
        {
            this.FakeHttpContext();

            var bld = new ContainerBuilder();
            bld.Register<ClassA>().PerRequest();

            var cnt = bld.Build();
            var cl = cnt.Resolve<ClassA>();
            var cl2 = cnt.Resolve<ClassA>();

            Assert.AreSame(cl, cl2);

        }

        private void FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://mySomething/", "");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                                 new HttpStaticObjectsCollection(), 10, true,
                                                                 HttpCookieMode.AutoDetect,
                                                                 SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                                     null, CallingConventions.Standard,
                                                     new[] { typeof(HttpSessionStateContainer) },
                                                     null)
                                                .Invoke(new object[] { sessionContainer });

            HttpContext.Current = httpContext;
        }
    }
}
