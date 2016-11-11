using System.Web.Mvc;
using MyAB.Core;

namespace MyAB.Web
{
    /// <summary>
    /// Includes the feature service in the controller's view bag.
    /// </summary>
    public class FeatureFilter : IActionFilter
    {
        private readonly IFeatureAvailabilityService _featureAvailabilityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureFilter"/> class using the specified
        /// feature service.
        /// </summary>                
        public FeatureFilter(IFeatureService featureService, ISystemClock clock)
        {
            Ensure.ArgumentNotNull(featureService, "featureService");
            Ensure.ArgumentNotNull(clock, "clock");
            _featureAvailabilityService = new FeatureAvailabilityService(featureService, clock);
        }

        /// <inheritdoc />
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        /// <inheritdoc />
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Features = _featureAvailabilityService;
        }
    }
}