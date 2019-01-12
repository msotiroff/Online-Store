using MSToolKit.Core.Extensions;
using MSToolKit.Core.Validation.Exceptions;
using System;

namespace MSToolKit.Core.Validation
{
    /// <summary>
    /// Provides validation methods for different kind of objects.
    /// This class cannot be inherited.
    /// </summary>
    public static class CoreValidator
    {
        /// <summary>
        /// Throws an ValidationFailedException if the input object is not in a valid state, depending on its validation attributes.
        /// </summary>
        /// <param name="input">
        /// The object that should be validated.
        /// </param>
        public static void ThrowIfInvalidState(object input)
        {
            var validationResult = input.GetValidationResult();

            if (validationResult.Success)
            {
                return;
            }

            throw new ValidationFailedException(string.Join(", ", validationResult.Errors));
        }

        /// <summary>
        /// Throws a System.ArgumentNullException if the input object is null.
        /// </summary>
        /// <param name="input">
        /// The object that should be validated.
        /// </param>
        public static void ThrowIfNull(object input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(
                    $"Parameter of type {input.GetType().Name} cannot be null.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentNullException if any of the input objects is null.
        /// </summary>
        /// <param name="input">The objects that should be validated.</param>
        public static void ThrowIfAnyNull(params object[] input)
        {
            foreach (var item in input)
            {
                ThrowIfNull(item);
            }
        }

        /// <summary>
        /// Throws a System.ArgumentNullException if the input string is null or empty.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNullOrEmpty(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(
                    "Parameter of type System.String cannot be null or empty.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentNullException if any of the input strings is null or empty.
        /// </summary>
        /// <param name="input">The objects that should be validated.</param>
        public static void ThrowIfAnyNullOrEmpty(params string[] inputs)
        {
            foreach (var input in inputs)
            {
                ThrowIfNullOrEmpty(input);
            }
        }

        /// <summary>a System.ArgumentNullException
        /// Throws a System.ArgumentNullException if the input string is null, empty or whitespace.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNullOrWhitespace(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(
                    "Parameter of type System.String cannot be null, empty or whitespace.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentNullException if any of the input strings is null, empty or whitespace.
        /// </summary>
        /// <param name="input">The objects that should be validated.</param>
        public static void ThrowIfAnyNullOrWhitespace(params string[] inputs)
        {
            foreach (var input in inputs)
            {
                ThrowIfNullOrWhitespace(input);
            }
        }

        /// <summary>
        /// Returns the passed object if it is not null. 
        /// If it's null - throws a System.ArgumentNullException.
        /// </summary>
        /// <typeparam name="TSource">The type of the input object.</typeparam>
        /// <param name="source">The object that should be validated and returned.</param>
        /// <returns>
        /// The validated source object.
        /// </returns>
        public static TSource ReturnOrThrowIfNull<TSource>(TSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(
                    $"Parameter of type {source.GetType().Name} cannot be null.");
            }

            return source;
        }

        /// <summary>
        /// Throws a System.ArgumentException if the passed object has its default value. 
        /// Null for reference types and default value for value types.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type if the object that should be validated.
        /// </typeparam>
        /// <param name="source">
        /// The object that should be validated.
        /// </param>
        public static void ThrowIfDefault<TSource>(TSource source) 
            where TSource : struct
        {
            if (source.Equals(default(TSource)))
            {
                throw new ArgumentException(
                    $"Parameter of type {source.GetType().Name} cannot be its default value.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentException if the passed object has negative value. Accepts System.Int32.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNegative(int input)
        {
            if (input < 0)
            {
                throw new ArgumentException(
                     $"Parameter of type {input.GetType().Name} cannot be negative.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentException if the passed object has negative value. Accepts System.Double.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNegative(double input)
        {
            if (input < 0)
            {
                throw new ArgumentException(
                     $"Parameter of type {input.GetType().Name} cannot be negative.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentException if the passed object has negative value. Accepts System.Decimal.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNegative(decimal input)
        {
            if (input < 0)
            {
                throw new ArgumentException(
                     $"Parameter of type {input.GetType().Name} cannot be negative.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentException if the passed object has negative value or zero. Accepts System.Int32.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNotPositive(int input)
        {
            if (input <= 0)
            {
                throw new ArgumentException(
                     $"Parameter of type {input.GetType().Name} should have a positive value.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentException if the passed object has negative value or zero. Accepts System.Double.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNotPositive(double input)
        {
            if (input <= 0)
            {
                throw new ArgumentException(
                     $"Parameter of type {input.GetType().Name} should have a positive value.");
            }
        }

        /// <summary>
        /// Throws a System.ArgumentException if the passed object has negative value or zero. Accepts System.Decimal.
        /// </summary>
        /// <param name="input">The object that should be validated.</param>
        public static void ThrowIfNotPositive(decimal input)
        {
            if (input <= 0)
            {
                throw new ArgumentException(
                     $"Parameter of type {input.GetType().Name} should have a positive value.");
            }
        }
    }
}
