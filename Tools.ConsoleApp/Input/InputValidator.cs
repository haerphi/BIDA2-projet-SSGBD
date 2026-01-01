using System.Numerics;
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

        public static Func<int?, string?> Range(int min, int max)
        {
            return (val) =>
            {
                return (val == null || val < min || val > max) ? $"out_of_range.{min}.{max}" : null;
            };
        }

       public static Func<int?, string?> GreaterThan(int threshold, bool equals = false)
        {
            return (val) =>
            {
                if (val == null)
                {
                    return "null";
                }
                if (equals)
                {
                    return (val < threshold) ? $"not_greater_than_or_equal.{threshold}" : null;
                }
                else
                {
                    return (val <= threshold) ? $"not_greater_than.{threshold}" : null;
                }
            };
        }

        public static Func<int?, string?> LowerThan(int threshold, bool equals = false)
        {
            return (val) =>
            {
                if (val == null)
                {
                    return "null";
                }
                if (equals)
                {
                    return (val > threshold) ? $"not_lower_than_or_equal.{threshold}" : null;
                }
                else
                {
                    return (val >= threshold) ? $"not_lower_than.{threshold}" : null;
                }
            };
        }
        #endregion
    }
}