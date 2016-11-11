namespace MyAB.Core
{
    /// <summary>
    /// Defines basic operations available for features.
    /// </summary>    
    public interface IFeatureService
    {
        /// <summary>
        /// Gets the hash code of the provided feature.
        /// </summary>
        int GetHashCode(Feature feature);

        /// <summary>
        /// Returns the status of the provided feature.
        /// </summary>
        bool IsAvailable(Feature feature);
    }
}