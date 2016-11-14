using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace SampleApp.WindsorInstallers
{
    [ExcludeFromCodeCoverage]
    public class WebInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblyFilter = new AssemblyFilter("bin", "MyAB.*");
            container.Register(
                Classes.FromAssemblyInDirectory(assemblyFilter)
                    .BasedOn<IHttpController>()
                    .LifestyleScoped(),
                Classes.FromAssemblyInDirectory(assemblyFilter)
                    .BasedOn<Controller>()
                    .LifestylePerWebRequest(),
                Component.For<HttpContextBase>()
                    .LifestylePerWebRequest()
                    .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));
        }
    }
}