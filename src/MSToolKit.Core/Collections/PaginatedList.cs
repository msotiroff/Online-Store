using MSToolKit.Core.Collections.Abstraction;
using MSToolKit.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MSToolKit.Core.Collections
{
    public class PaginatedList<T> : IPaginatedList, IEnumerable<T> where T : class
    {
        private readonly IEnumerable<T> data;

        public PaginatedList(IQueryable<T> allItems, int pageIndex, int itemsPerPage)
        {
            CoreValidator.ThrowIfNotPositive(itemsPerPage);

            this.TotalPages = this.CalculateTotalPages(allItems, itemsPerPage);
            this.PageIndex = this.GetValidPageIndex(pageIndex);
            this.data = this.GetPaginatedData(allItems, pageIndex, itemsPerPage);
        }

        /// <summary>
        /// Gets the current page index
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Gets the total pages count.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Returns true of false, depending on that the collection has a previous page or not.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Returns true of false, depending on that the collection has a next page or not.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        public IEnumerator<T> GetEnumerator() => this.data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        
        private int CalculateTotalPages(IQueryable<T> allItems, int itemsPerPage)
        {
            return (int)Math.Ceiling(allItems.Count() / (double)itemsPerPage);
        }

        private int GetValidPageIndex(int pageIndex)
        {
            if (pageIndex < 1)
            {
                return 1;
            }

            if (pageIndex > this.TotalPages)
            {
                return this.TotalPages;
            }

            return pageIndex;
        }

        private IEnumerable<T> GetPaginatedData(IQueryable<T> allItems, int pageIndex, int itemsPerPage)
        {
            return allItems
                .Skip((pageIndex - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }
    }
}
