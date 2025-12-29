using System.Text.RegularExpressions;

namespace Tools.ConsoleApp.Input
{
    public static class InputValidator
    {
        #region General validators
        public static Func<T?, string?> IsNotNull<T>() where T : class
        {
            return (input) =>
            {
                return input is null ? "null" : null;
            };
        }

        public static Func<T?, string?> IsIn<T>(IEnumerable<T?> possibleValues)
        {
            return (input) =>
            {
                return !possibleValues.Contains(input) ? "not_in_list" : null;
            };
        }
        #endregion

        #region String validators
        public static Func<string?, string?> IsNotEmpty()
        {
            return (input) =>
            {
                return string.IsNullOrWhiteSpace(input) ? "empty" : null;
            };
        }

        public static Func<string?, string?> MinLength(int min)
        {
            return (input) =>
            {
                return (input == null || input.Length < min) ? $"min_length.{min}" : null;
            };
        }

        public static Func<string?, string?> Match(string pattern)
        {
            return (input) =>
            {
                return (input == null || !Regex.IsMatch(input, pattern)) ? "pattern_not_matched" : null;
            };
        }
        #endregion

        #region Number validators
        public static Func<int?, string?> IsPositive()
        {
            return (val) =>
            {
                return (val == null || val <= 0) ? "not_positive" : null;
            };
        }

        public static Func<T, string?> Range<T>(T min, T max) where T : IComparable<T>
        {
            return (val) =>
            {
                return (val.CompareTo(min) < 0 || val.CompareTo(max) > 0) ? $"range_out.{min}-{max}" : null;
            };
        }

        public static Func<T, string?> GreaterThan<T>(T threshold, bool orEqual = false) where T : IComparable<T>
        {
            return (val) =>
            {
                bool isLower = orEqual ? val.CompareTo(threshold) < 0 : val.CompareTo(threshold) <= 0;
                return isLower ? $"must_be_greater_than.{threshold}" : null;
            };
        }

        public static Func<T, string?> LessThan<T>(T threshold, bool orEqual = false) where T : IComparable<T>
        {
            return (val) =>
            {
                bool isHigher = orEqual ? val.CompareTo(threshold) > 0 : val.CompareTo(threshold) >= 0;
                return isHigher ? $"must_be_less_than.{threshold}" : null;
            };
        }
        #endregion
    }
}