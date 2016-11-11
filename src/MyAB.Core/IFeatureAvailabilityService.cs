namespace MyAB.Core
{
    /// <summary>
    /// Defines the basic contract for feature availability
    /// </summary>
    public interface IFeatureAvailabilityService
    {
        bool IsAvailable(Feature feature);
    }
}
