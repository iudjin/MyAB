using System;
using System.Configuration;
using System.Web;
using MyAB.Core;

namespace MyAB.Web
{
    public class FeatureAvailabilityService : IFeatureAvailabilityService
    {
        private readonly IFeatureService _featureService;
        private readonly ISystemClock _clock;
        private readonly string _workFeatureCookieName;
        private readonly string _workFeatureCookieHashName;

        public FeatureAvailabilityService(IFeatureService featureService, ISystemClock clock)
        {
            Ensure.ArgumentNotNull(featureService, "featureService");
            Ensure.ArgumentNotNull(clock, "clock");
            _featureService = featureService;
            _clock = clock;
            _workFeatureCookieName = string.Format(FeatureConsts.WorkFeatureCookieTemplateName, ConfigurationManager.AppSettings["ABTestingFeatureTag"]);
            _workFeatureCookieHashName = string.Format(FeatureConsts.WorkFeatureHashCookieTemplateName, ConfigurationManager.AppSettings["ABTestingFeatureTag"]);
        }

        public bool IsAvailable(Feature feature)
        {
            if (ConfigurationManager.AppSettings["EnableABTesting"] == "false")
                return false;

            string featureName = feature.ToString();

            var featureCookie = HttpContext.Current.Request.Cookies.Get(_workFeatureCookieName);
            var featureHashCookie = HttpContext.Current.Request.Cookies.Get(_workFeatureCookieHashName);

            if (featureHashCookie != null)
            {
                if (featureCookie == null)
                {
                    featureCookie = CreateFeatureCookie();
                }

                var featureItemValue = featureCookie.Values[featureName];
                if (featureItemValue != null)
                {
                    return featureItemValue.Split(':')[1] == "True";
                }

                return AddFeature(feature, featureCookie);
            }
            else
            {
                featureHashCookie = new HttpCookie(_workFeatureCookieHashName) { Value = "True" };
                HttpContext.Current.Response.SetCookie(featureHashCookie);
                featureHashCookie.Expires = _clock.Now.AddHours(int.Parse(ConfigurationManager.AppSettings["ABTestingFeatureHashExpirationInHours"] ?? FeatureConsts.ABTestingFeatureHashExpirationInHoursDefault));

                if (featureCookie == null)
                {
                    featureCookie = CreateFeatureCookie();
                    return AddFeature(feature, featureCookie);
                }

                bool needToExtendLifetimeOfTheFeatureCookie = false;
                var featureKeys = featureCookie.Values.AllKeys;

                foreach (string key in featureKeys)
                {
                    Feature featureItem;
                    if (Enum.TryParse(key, out featureItem))
                    {
                        var hash = _featureService.GetHashCode(featureItem).ToString();
                        if (featureCookie.Values[key].Split(':')[0] != hash)
                        {
                            featureCookie.Values.Set(key, string.Format("{0}:{1}", hash, _featureService.IsAvailable(featureItem)));
                            needToExtendLifetimeOfTheFeatureCookie = true;
                        }
                    }
                    else
                    {
                        featureCookie.Values.Remove(key);
                        HttpContext.Current.Response.SetCookie(featureCookie);
                    }
                }

                if (needToExtendLifetimeOfTheFeatureCookie)
                {
                    featureCookie.Expires = _clock.Now.AddDays(int.Parse(ConfigurationManager.AppSettings["ABTestingFeatureExpirationInDays"] ?? FeatureConsts.ABTestingFeatureExpirationInDaysDefault));
                    HttpContext.Current.Response.SetCookie(featureCookie);
                }

                var featureItemValue = featureCookie.Values[featureName];
                if (featureItemValue != null)
                {
                    return featureItemValue.Split(':')[1] == "True";
                }

                return AddFeature(feature, featureCookie);
            }
        }

        private bool AddFeature(Feature feature, HttpCookie featureCookie)
        {
            bool isAvailable = _featureService.IsAvailable(feature);
            featureCookie.Values.Add(feature.ToString(), string.Format("{0}:{1}", _featureService.GetHashCode(feature), isAvailable));
            HttpContext.Current.Response.SetCookie(featureCookie);
            return isAvailable;
        }

        private HttpCookie CreateFeatureCookie()
        {
            HttpCookie featureCookie = new HttpCookie(_workFeatureCookieName)
            {
                Expires =
                    _clock.Now.AddDays(
                        int.Parse(ConfigurationManager.AppSettings["ABTestingFeatureExpirationInDays"] ?? FeatureConsts.ABTestingFeatureExpirationInDaysDefault))
            };
            HttpContext.Current.Response.SetCookie(featureCookie);
            return featureCookie;
        }
    }
}
