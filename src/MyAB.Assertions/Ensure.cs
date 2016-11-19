using System;
using System.Diagnostics.CodeAnalysis;

namespace MyAB.Assertions
{
    /// <summary>
    /// Provides runtime assertions for common method preconditions.
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Throws an exception if an argument is less than or equal to a specified minimum value.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="minValue">The minimum value allowed (exclusive).</param>
        /// <param name="paramName">The argument name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>value</c> is
        /// less than or equal to <paramref name="minValue"/>.</exception>        
        [ExcludeFromCodeCoverage]
        public static void ArgumentGreater<T>(T value, T minValue, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(minValue) <= 0)
                throw new ArgumentOutOfRangeException(paramName, value,
                    string.Format("Argument must be greater than the minimum value of {0}.", 
                        minValue));
        }
        
        /// <summary>
        /// Throws an exception if an argument is bigger than specified maximum value.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="maxValue">The maximum value allowed (exclusive).</param>
        /// <param name="paramName">The argument name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>value</c> is
        /// bigger than <paramref name="maxValue"/>.</exception>        
        [ExcludeFromCodeCoverage]
        public static void ArgumentSmaller<T>(T value, T maxValue, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(maxValue) >= 0)
                throw new ArgumentOutOfRangeException(paramName, value,
                    string.Format("Argument must be smaller than the maximum value of {0}.", 
                        maxValue));
        }
        
        /// <summary>
        /// Throws an exception if an argument is less than or equal to a specified minimum value.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="paramName">The argument name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>value</c> does
        /// equal to <paramref name="expectedValue"/>.</exception>        
        [ExcludeFromCodeCoverage]
        public static void ArgumentEquals<T>(T value, T expectedValue, string paramName)
            where T : IComparable<T>
        {
            if (!value.Equals(expectedValue))
                throw new ArgumentOutOfRangeException(paramName, value,
                    string.Format("Argument must equal to value of {0}.",
                        expectedValue));
        }

        /// <summary>
        /// Throws an exception if an argument is less than a specified minimum value.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="minValue">The minimum value allowed (inclusive).</param>
        /// <param name="paramName">The argument name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>value</c> is
        /// less than <paramref name="minValue"/>.</exception>        
        [ExcludeFromCodeCoverage]
        public static void ArgumentGreaterOrEqual<T>(T value, T minValue, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(minValue) < 0)
                throw new ArgumentOutOfRangeException(paramName, value,
                    string.Format("Argument must be greater than or equal to the minimum value " +
                        "of {0}.", minValue));
        }
        
        /// <summary>
        /// Throws an exception if an argument is bigger than a specified maximum value.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="maxValue">The maximum value allowed (inclusive).</param>
        /// <param name="paramName">The argument name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>value</c> is
        /// bigger than <paramref name="maxValue"/>.</exception>        
        [ExcludeFromCodeCoverage]
        public static void ArgumentSmallerOrEqual<T>(T value, T maxValue, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(maxValue) > 0)
                throw new ArgumentOutOfRangeException(paramName, value,
                    string.Format("Argument must be lesser than or equal to the maximum value " +
                        "of {0}.", maxValue));
        }

        /// <summary>
        /// Throws an exception if an argument is <c>null</c>.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="paramName">The argument name.</param>
        /// <exception cref="ArgumentNullException">Thrown when <c>value</c> is
        /// <c>null</c>.</exception>        
        [ExcludeFromCodeCoverage]
        public static void ArgumentNotNull(object value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Throws an exception if a string argument is <c>null</c> or empty.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="paramName">The argument name.</param>
        /// <exception cref="ArgumentNullException">Thrown when <c>value</c> is
        /// <c>null</c> or empty.</exception>        
        [ExcludeFromCodeCoverage]
        public static void ArgumentNotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
            if (value == string.Empty)
                throw new ArgumentException(string.Format("Value for parameter '{0}' cannot be " +
                    "an empty string.", paramName), paramName);
        }
    }
}