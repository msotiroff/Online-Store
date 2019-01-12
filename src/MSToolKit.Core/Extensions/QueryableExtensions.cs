using MSToolKit.Core.Collections;
using MSToolKit.Core.Collections.Abstraction;
using System.Linq;
using System.Linq.Expressions;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for using over System.Linq.IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {
        private const string OrderBy = "OrderBy";
        private const string OrderByDescending = "OrderByDescending";
        private const string ThenBy = "ThenBy";
        private const string ThenByDescending = "ThenByDescending";
        
        /// <summary>
        /// Returns new instance of PaginatedList with the given data.
        /// </summary>
        /// <typeparam name="T">
        /// The generic type, that should be paginated.
        /// </typeparam>
        /// <param name="source">System.Linq.IQueryable with all items.</param>
        /// <param name="pageIndex">The current page index, that should be returned.</param>
        /// <param name="itemsPerPage">The count of items per page, that should be returned.</param>
        /// <returns>
        /// New instance for MSToolKit.Core.Collections.Abstraction.IPaginatedList with the given data.
        /// </returns>
        public static PaginatedList<T> ToPaginatedList<T>(
            this IQueryable<T> source, int pageIndex, int itemsPerPage)
            where T : class
        {
            return new PaginatedList<T>(source, pageIndex, itemsPerPage);
        }

        /// <summary>
        /// Sort the given queriable collection in ascending order by a specified member.
        /// </summary>
        /// <typeparam name="T">The generic type of the collection.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="member">The specified member to order by.</param>
        /// <returns>The sorted source as System.Linq.IOrderedQueryable.</returns>
        public static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string member)
        {
            return source.OrderByMemberUsing(member, OrderBy);
        }

        /// <summary>
        /// Sort the given queriable collection in descending order by a specified member.
        /// </summary>
        /// <typeparam name="T">The generic type of the collection.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="member">The specified member to order by.</param>
        /// <returns>The sorted source as System.Linq.IOrderedQueryable.</returns>
        public static IOrderedQueryable<T> OrderByMemberDescending<T>(this IQueryable<T> source, string member)
        {
            return source.OrderByMemberUsing(member, OrderByDescending);
        }

        /// <summary>
        /// Sort the given sorted queriable collection in ascending order by a specified member as next order member.
        /// </summary>
        /// <typeparam name="T">The generic type of the collection.</typeparam>
        /// <param name="source">The source ordered queryable collection.</param>
        /// <param name="member">The specified member to order by.</param>
        /// <returns>The sorted source as System.Linq.IOrderedQueryable.</returns>
        public static IOrderedQueryable<T> ThenByMember<T>(this IOrderedQueryable<T> source, string member)
        {
            return source.OrderByMemberUsing(member, ThenBy);
        }

        /// <summary>
        /// Sort the given sorted queriable collection in descending order by a specified member as next order member.
        /// </summary>
        /// <typeparam name="T">The generic type of the collection.</typeparam>
        /// <param name="source">The source ordered queryable collection.</param>
        /// <param name="member">The specified member to order by.</param>
        /// <returns>The sorted source as System.Linq.IOrderedQueryable.</returns>
        public static IOrderedQueryable<T> ThenByMemberDescending<T>(this IOrderedQueryable<T> source, string member)
        {
            return source.OrderByMemberUsing(member, ThenByDescending);
        }

        private static IOrderedQueryable<T> OrderByMemberUsing<T>(this IQueryable<T> source, string memberPath, string method)
        {
            var isOrderMemberValid = typeof(T)
                .GetProperties()
                .Any(pi => pi.Name == memberPath);

            if (!isOrderMemberValid)
            {
                return source as IOrderedQueryable<T>;
            }

            var parameter = Expression.Parameter(typeof(T), "item");

            var member = memberPath.Split('.')
                .Aggregate((Expression)parameter, Expression.PropertyOrField);

            var keySelector = Expression.Lambda(member, parameter);

            var methodCall = Expression.Call(
                typeof(Queryable), method,
                new[] { parameter.Type, member.Type },
                source.Expression, Expression.Quote(keySelector));

            var destination = source.Provider.CreateQuery(methodCall);

            return destination as IOrderedQueryable<T>;
        }
    }
}