namespace MyAB.Core
{
    public abstract class FeatureConsts
    {
        public const string ABTestingFeatureHashExpirationInHoursDefault = "24";
        public const string ABTestingFeatureExpirationInDaysDefault = "60";
        public const string WorkFeatureCookieTemplateName = "WorkFeature_{0}";
        public const string WorkFeatureHashCookieTemplateName = "WorkFeatureHash_{0}";
    }
}