namespace Tools.ConsoleApp.Input
{
    public static class InputExitCondition
    {
        public static bool IsQuitCommand(string input)
        {
            return string.Equals(input, "quit", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCancelCommand(string input)
        {
            return string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsEmptyInput(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
    }
}
