using System;
using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MyAB.Core;
using MyAB.Web;
using NFeature;
using NFeature.Configuration;
using NFeature.DefaultImplementations;

namespace MyAB.SampleApp.WindsorInstallers
{
    [ExcludeFromCodeCoverage]
    public class NFeatureInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Func<FeatureSetting<Feature, DefaultTenantEnum>, EmptyArgs, bool> checkAvailability =
                (f, args) => NFeatureService.IsAvailable(f,
                    Tuple.Create(FeatureVisibilityMode.Normal, DateTime.Now));
            container.Register(
                Component.For<IFeatureSettingRepository<Feature>>()
                    .ImplementedBy<AppConfigFeatureSettingRepository<Feature>>()
                    .LifestyleSingleton()
                    .OnlyNewServices(),
                Component.For<IFeatureSettingAvailabilityChecker<Feature>>()
                    .ImplementedBy<FeatureSettingAvailabilityChecker<Feature>>()
                    .DependsOn(new { availabilityCheckFunction = checkAvailability })
                    .LifestyleSingleton()
                    .OnlyNewServices(),
                Component.For<IFeatureSettingService<Feature>>()
                    .ImplementedBy<FeatureSettingService<Feature>>()
                    .LifestyleSingleton()
                    .OnlyNewServices(),
                Component.For<IFeatureManifestCreationStrategy<Feature>>()
                    .ImplementedBy<ManifestCreationStrategyDefault<Feature>>()
                    .LifestyleSingleton()
                    .OnlyNewServices(),
                Component.For<IFeatureManifestService<Feature>>()
                    .ImplementedBy<FeatureManifestService<Feature>>()
                    .LifestyleSingleton()
                    .OnlyNewServices(),
                Component.For<IFeatureManifest<Feature>>()
                    .UsingFactoryMethod((kernel, context) =>
                        kernel.Resolve<IFeatureManifestService<Feature>>().GetManifest())
                    .LifestyleScoped()
                    .OnlyNewServices()
            );
            container.Register(Component.For<IFeatureService>()                
                .LifeStyle.Singleton
                .OnlyNewServices()
                .ImplementedBy<NFeatureService>());
            container.Register(Component.For<IFeatureAvailabilityService>()
                .LifeStyle.Singleton
                .OnlyNewServices()
                .ImplementedBy<FeatureAvailabilityService>());
        }
    }
}