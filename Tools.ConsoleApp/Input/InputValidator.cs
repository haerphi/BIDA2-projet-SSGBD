using System.Text.RegularExpressions;

namespace Tools.ConsoleApp.Input
{
    public static class InputValidator
    {
        // String validators
        public static string? IsNotEmpty(string input)
            => string.IsNullOrWhiteSpace(input) ? "empty" : null;

        public static Func<string, string?> MinLength(int min)
        {
            return (input) => input.Length < min
                ? $"min_length.{min}"
                : null;
        }

        public static Func<string, string?> Match(string pattern)
        {
            return (input) => !Regex.IsMatch(input, pattern)
                ? "match"
                : null;
        }

        // int validators
        public static Func<int, string?> IsPositive() =>
        (val) => val < 0 ? "must_be_positive" : null;

        public static Func<int, string?> Range(int min, int max) =>
            (val) => (val < min || val > max) ? $"range_out.{min}-{max}" : null;

        public static Func<int, string?> GreaterThan(int min, bool equal = false) =>
            (val) => (equal ? val < min : val <= min) ? $"must_be_greater_than.{min}" : null;

        public static Func<int, string?> LessThan(int max, bool equal = false) =>
            (val) => (equal ? val > max : val >= max) ? $"must_be_less_than.{max}" : null;
    }
}