using MyAB.Core;

namespace MyAB.Web
{
    /// <summary>
    /// Base class for all Web controls within the site.
    /// </summary> 
    public abstract class ViewUserControl<TModel> : System.Web.Mvc.ViewUserControl<TModel>
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
    /// Base class for all Web controls within the site.
    /// </summary> 
    public abstract class ViewUserControl : System.Web.Mvc.ViewUserControl
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