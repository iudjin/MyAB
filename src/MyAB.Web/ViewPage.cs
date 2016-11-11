using MyAB.Core;

namespace MyAB.Web
{
    /// <summary>
    /// Base class for all Web pages within the site.
    /// </summary> 
    public abstract class ViewPage<TModel> : System.Web.Mvc.ViewPage<TModel>
    {
        /// <summary>
        /// Checks if a specific feature is available/enabled.
        /// </summary>
        /// <param name="feature">The feature toggle to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="feature"/> is available;
        /// otherwise, <c>false</c>.</returns>        
        public bool IsAvailable(Feature feature)
        {
            var featureAvailiabilityService = (IFeatureAvailabilityService)ViewBag.Features;
            if (featureAvailiabilityService == null)
                return false;
            return featureAvailiabilityService.IsAvailable(feature);
        }
    }

    /// <summary>
    /// Base class for all Web pages within the site.
    /// </summary> 
    public abstract class ViewPage : System.Web.Mvc.ViewPage
    {
        /// <summary>
        /// Checks if a specific feature is available/enabled.
        /// </summary>
        /// <param name="feature">The feature toggle to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="feature"/> is available;
        /// otherwise, <c>false</c>.</returns>
        public bool IsAvailable(Feature feature)
        {
            var featureAvailiabilityService = (IFeatureAvailabilityService)ViewBag.Features;
            if (featureAvailiabilityService == null)
                return false;
            return featureAvailiabilityService.IsAvailable(feature);
        }
    }
}