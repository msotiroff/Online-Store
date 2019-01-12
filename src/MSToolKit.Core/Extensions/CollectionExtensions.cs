using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for using over collections.
    /// </summary>
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return true;
            }

            if (collection.Count() == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Projects in parallel each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of elements to invoke a transform function on.</typeparam>
        /// <typeparam name="TDestination">The type of elements resturned by selector.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="action">A transform function to apply to each element.</param>
        /// <returns>
        /// A sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        public static IEnumerable<TDestination> SelectAsync<TSource, TDestination>(
            this IEnumerable<TSource> source, Func<TSource, Task<TDestination>> action)
        {
            var destinationCollection = new List<TDestination>();
            
            Parallel.ForEach(source, item => 
            {
                destinationCollection.Add(action(item).GetAwaiter().GetResult());
            });

            return destinationCollection;
        }

        /// <summary>
        /// Returns true or false depending of that the given collection contains at least 
        /// as many passed elements as the number given such first parameter.
        /// </summary>
        /// <typeparam name="T">IComparable object type.</typeparam>
        /// <param name="collection">The collection to be checked for.</param>
        /// <param name="count">Minimum count of occurences.</param>
        /// <param name="args">The elements to check for.</param>
        /// <returns>True or False, depending on the result.</returns>
        public static bool ContainsFew<T>(this IEnumerable<T> collection, int count, params T[] args)
            where T : IComparable<T>
        {
            if (count <= 0)
            {
                throw new ArgumentException($"Argument \"{nameof(count)}\" should be a positive integer!");
            }
            if (count > args.Length)
            {
                throw new ArgumentException("The count of given arguments cannot be less than the first parameter");
            }

            foreach (var arg in args)
            {
                if (collection.Contains(arg))
                {
                    count--;
                }
            }

            return count <= 0;
        }

        /// <summary>
        /// Returns true or false depending of that the given collection contains all the passed arguments.
        /// </summary>
        /// <typeparam name="T">IComparable object type.</typeparam>
        /// <param name="collection">The collection to be checked for.</param>
        /// <param name="args">The elements to check for.</param>
        /// <returns>True or False, depending on the result.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> collection, params T[] args)
            where T : IComparable<T>
        {
            foreach (var item in args)
            {
                if (!collection.Any(i => i.Equals(item)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Allows all collections, that implement IEnumerable to use ForEach with lambda expression. 
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="collection">The collection, that should be enumerated.</param>
        /// <param name="action">
        /// Action delegate that should be performed to the each element of the given collection.
        /// </param>
        /// <returns>The modified collection.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }

            return collection;
        }

        /// <summary>
        /// Returns new instance of Queue with all elements from the given collection.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="collection">The source collection.</param>
        /// <returns>System.Collections.Generic.Queue</returns>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> collection)
        {
            return new Queue<T>(collection);
        }

        /// <summary>
        /// Returns new instance of Stack with all elements from the given collection.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="collection">The source collection.</param>
        /// <returns>System.Collections.Generic.Stack</returns>
        public static Stack<T> ToStack<T>(this IEnumerable<T> collection)
        {
            return new Stack<T>(collection);
        }
    }
}
