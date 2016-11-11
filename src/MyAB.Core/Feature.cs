namespace MyAB.Core
{
    /// <summary>
    /// Defines the current feature toggles for the application.
    /// </summary>
    public enum Feature
    {
        TestOnly,
        JobSearchElasticSearch,
        JobSearchElasticSearchVariantB,
        ShowOneFeaturedJob,
        ShowNewTypography,
        ReduceAvailableFilters,
        ShowExpandedFilters,
        FacetsOnLeft
    }
}
