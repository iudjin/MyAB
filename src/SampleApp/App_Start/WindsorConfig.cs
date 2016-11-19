using System.Web.Http;
using System.Web.Mvc;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using SampleApp.WindsorInstallers;
using Web.Windsor;

namespace SampleApp
{
    public static class WindsorConfig
    {
        public static WindsorContainer Register(HttpConfiguration config)
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Kernel.Resolver.AddSubResolver(new 
                CollectionResolver(container.Kernel, true));
            config.DependencyResolver = new WindsorHttpDependencyResolver(container);
            DependencyResolver.SetResolver(new WindsorMvcDependencyResolver(container));            
            container.Install(new NFeatureInstaller());
            var assemblyFilter = new AssemblyFilter("bin", "MyAB.*");
            container.Install(FromAssembly.InDirectory(assemblyFilter));            
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            return container;
        }
    }
}