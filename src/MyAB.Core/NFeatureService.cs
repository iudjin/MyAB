using System;
using System.Linq;
using System.Text;
using NFeature;
using NFeature.Configuration;
using NFeature.DefaultImplementations;

namespace MyAB.Core
{
    /// <summary>
    /// NFeature specific implementation
    /// </summary>
    public class NFeatureService : IFeatureService
    {
        private readonly IFeatureManifestService<Feature> _manifestService;
        private readonly FeatureSetting<Feature, DefaultTenantEnum>[] _featureSettings;

        public NFeatureService(IFeatureManifestService<Feature> manifestService, IFeatureSettingRepository<Feature> featureSettingRepository)
        {
            Ensure.ArgumentNotNull(manifestService, "manifestService");
            Ensure.ArgumentNotNull(featureSettingRepository, "featureSettingRepository");

            _manifestService = manifestService;
            _featureSettings = featureSettingRepository.GetFeatureSettings();
        }

        public int GetHashCode(Feature feature)
        {
            var s = _featureSettings.FirstOrDefault(x => x.Feature == feature);

            var hashString = new StringBuilder();
            hashString
                .Append(s.Feature)
                .Append(s.FeatureState)
                .Append(s.EndDtg)
                .Append(s.StartDtg);

            foreach (var d in s.Dependencies)
            {
                hashString.Append(d);
            }

            foreach (var st in s.SupportedTenants)
            {
                hashString.Append(st);
            }

            foreach (var setting in s.Settings)
            {
                hashString.Append(string.Format("{0}:{1}", setting.Key, setting.Value));
            }

            return hashString.ToString().GetHashCode();
        }

        public bool IsAvailable(Feature feature)
        {
            return feature.IsAvailable(_manifestService.GetManifest());
        }

        /// <summary>
        /// NFeature specific extension of availability function
        /// </summary>
        /// <typeparam name="TFeatureEnum"></typeparam>
        /// <param name="s"></param>
        /// <param name="args"></param>
        /// <returns>status of the feature</returns>
        public static bool IsAvailable<TFeatureEnum>(
            FeatureSetting<TFeatureEnum, DefaultTenantEnum> s,
            Tuple<FeatureVisibilityMode, DateTime> args)
            where TFeatureEnum : struct
        {
            bool isEnabled = DefaultFunctions.AvailabilityCheckFunction(s, Tuple.Create(FeatureVisibilityMode.Normal, DateTime.Now));
            if (isEnabled)
            {
                dynamic settingValue;
                if (s.Settings.TryGetValue(FeatureSettingNames.BasedOnRandomFactor.ToString(), out settingValue))
                {
                    var r = new Random();
                    if (settingValue <= r.Next(100))
                        return false;
                }

                return true;
            }

            return false;
        }
    }
}