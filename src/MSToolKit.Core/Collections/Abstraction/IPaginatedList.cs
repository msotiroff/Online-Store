namespace MSToolKit.Core.Collections.Abstraction
{
    /// <summary>
    /// Provides an abstraction for pagination.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type, that should be paginated.
    /// </typeparam>
    public interface IPaginatedList
    {
        /// <summary>
        /// Gets the current page index
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Gets the total pages count.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Returns true of false, depending on that the collection has a previous page or not.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Returns true of false, depending on that the collection has a next page or not.
        /// </summary>
        bool HasNextPage { get; }
    }
}
