using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;

namespace SampleApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        #region IContainerAccessor Members
        private static WindsorContainer _container;

        public IWindsorContainer Container
        {
            get { return _container; }
        }

        #endregion
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            _container = WindsorConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
