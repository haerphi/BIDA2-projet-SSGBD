namespace Tools.ConsoleApp.Input
{
    public static class Inputs
    {
        #region General errors
        public const string QUIT_ERROR = "quit";
        #endregion

        #region Number errors
        public const string INVALID_FORMAT_ERROR = "invalid_format";
        #endregion

        public static void DisplayErrors(IEnumerable<string> errors)
        {
            Console.WriteLine("Erreurs de saisie:");
            foreach (var error in errors)
            {
                Console.WriteLine($"- {error}");
            }
        }

        public static IEnumerable<string> ReadValue<T>(
            string prompt,
            Func<string, T?> parser,
            IEnumerable<Func<T, string?>> validators,
            out T? userInput,
            bool shouldRetry = true,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            List<string> errors;
            userInput = default;

            do
            {
                errors = new List<string>();
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                // Condition de sortie
                if (exitCondition != null && exitCondition(input))
                {
                    errors.Add(QUIT_ERROR);
                    break;
                }

                // Conversion via le parser
                try
                {
                    userInput = parser(input);
                    if (userInput == null && typeof(T).IsValueType)
                    {
                        errors.Add("invalid_format");
                    }
                }
                catch
                {
                    errors.Add("parse_error");
                }

                // Validation si aucune erreur de parsing
                if (!errors.Any())
                {
                    foreach (var validator in validators)
                    {
                        var error = validator(userInput!);
                        if (error != null) errors.Add(error);
                    }
                }

                // Affichage erreurs
                if (errors.Any() && displayError != null)
                {
                    displayError(errors);
                }

            } while (shouldRetry && errors.Any());

            return errors;
        }

        public static IEnumerable<string> ReadString(
            string prompt,
            IEnumerable<Func<string, string?>> validators,
            out string userInput,
            bool shouldRetry = true,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            var errors = ReadValue(
                prompt,
                input => input,
                validators,
                out userInput,
                shouldRetry,
                exitCondition,
                displayError
            );
            return errors;
        }

        public static IEnumerable<string> ReadInt(
            string prompt,
            IEnumerable<Func<int, string?>> validators,
            out int userInput,
            bool shouldRetry = true,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            var errors = ReadValue(
                prompt,
                input => int.TryParse(input, out int val) ? val : throw new Exception(),
                validators,
                out userInput,
                shouldRetry,
                exitCondition,
                displayError
            );
            return errors;
        }
    }
}