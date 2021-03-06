﻿using Range.Net.Abstractions;
using System;

namespace Range.Net
{
    public static class RangeExtensions
    {
        /// <summary>
        /// Determines if the provided value is inside the range
        /// </summary>
        /// <param name="range">The range to test</param>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public static bool Contains<T>(this IRange<T> range, T value)
            where T : IComparable<T>
        {
            var minInclusive = ((int) range.Inclusivity & 2) == 2; // If the second bit set then min is inclusive
            var maxInclusive = ((int) range.Inclusivity & 1) == 1; // If the first bit set then max is inclusive

            var testMin = minInclusive ? range.Minimum.CompareTo(value) <= 0 : range.Minimum.CompareTo(value) < 0;
            var testMax = maxInclusive ? range.Maximum.CompareTo(value) >= 0 : range.Maximum.CompareTo(value) > 0;

            return testMin && testMax;
        }

        /// <summary>
        /// Determines if another range is inside the bounds of this range
        /// </summary>
        /// <param name="range">The range to test</param>
        /// <param name="value">The value to test</param>
        /// <returns>True if range is inside, else false</returns>
        public static bool Contains<T>(this IRange<T> range, IRange<T> value)
            where T : IComparable<T>
        {
            return range.Contains(value.Minimum) // For when A contains B
                   || range.Contains(value.Maximum)
                   || value.Contains(range.Minimum) // For when B contains A
                   || value.Contains(range.Maximum);
        }

        /// <summary>
        /// Determines if another range intersects with this range.
        /// The either range may completely contain the other
        /// </summary>
        /// <param name="range">The range</param>
        /// <param name="value">The other range</param>
        /// <returns>True of the this range intersects the other range</returns>
        public static bool Intersects<T>(this IRange<T> range, IRange<T> value)
            where T : IComparable<T>
        {
            return
                range.Contains(value.Minimum) || // For when A contains B
                range.Contains(value.Maximum) ||
                value.Contains(range.Minimum) || // For when B contains A
                value.Contains(range.Maximum);
        }

        /// <summary>
        /// Create a union of two ranges so that a new range with the minimum of
        /// the minimum of both ranges and the maximum of the maximum of bother ranges
        /// </summary>
        /// <param name="range">A range with which to union</param>
        /// <param name="value">A range with which to union</param>
        /// <returns>A new range with the minimum of the minimum of both ranges and
        /// the maximum of the maximum of bother ranges</returns>
        public static IRange<T> Union<T>(this IRange<T> range, IRange<T> value)
            where T : IComparable<T>
        {
            return new Range<T>(
                range.Minimum.CompareTo(value.Minimum) < 0 ? range.Minimum : value.Minimum,
                range.Maximum.CompareTo(value.Maximum) > 0 ? range.Maximum : value.Maximum);
        }
    }
}