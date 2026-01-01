using System.Data.SqlTypes;

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

        public static void Pause(string text = "\nAppuyez sur une touche pour continuer...")
        {
            Console.WriteLine(text);
            Console.ReadKey();
        }

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
            IEnumerable<Func<T?, string?>> validators,
            out T? userInput,
            T? defaultValue = default,
            bool shouldRetry = true,
            IEnumerable<Func<string, string?>>? preValidators = null,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            List<string> errors;
            userInput = default;

            do
            {
                bool assigned = false;
                errors = new List<string>();

                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                // Condition de sortie
                if (exitCondition != null && exitCondition(input))
                {
                    errors.Add(QUIT_ERROR);
                    userInput = defaultValue;
                    assigned = true;
                    shouldRetry = false;
                }

                // valeur par défaut
                if (!errors.Any() && string.IsNullOrEmpty(input) && defaultValue is not null)
                {
                    userInput = defaultValue;
                    assigned = true;
                    shouldRetry = false;
                }

                // Validateurs d'input (string)
                if (!errors.Any() && !assigned && preValidators is not null)
                {
                    foreach (var preValidator in preValidators)
                    {
                        var error = preValidator(input);
                        if (error != null) errors.Add(error);
                    }
                }

                if (!assigned && !errors.Any())
                {
                    // Conversion de l'input
                    try
                    {
                        userInput = parser(input);
                        assigned = true;
                        if (userInput == null)
                        {
                            errors.Add(INVALID_FORMAT_ERROR);
                        }
                    }
                    catch
                    {
                        errors.Add("parse_error");
                    }

                    // Validateurs d'input (T)
                    if (!errors.Any())
                    {
                        foreach (var validator in validators)
                        {
                            var error = validator(userInput);
                            if (error != null) errors.Add(error);
                        }
                    }

                    // Affichage des erreurs
                    if (errors.Any() && displayError != null)
                    {
                        displayError(errors);
                    }

                    if (!errors.Any())
                    {
                        shouldRetry = false;
                    }
                }
            } while (shouldRetry);

            return errors;
        }

        public static IEnumerable<string> ReadString(
            string prompt,
            IEnumerable<Func<string?, string?>> validators,
            out string? userInput,
            string? defaultValue = null,
            bool shouldRetry = true,
            IEnumerable<Func<string, string?>>? preValidators = null,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            var errors = ReadValue(
                prompt: prompt,
                parser: input => input,
                validators: validators,
                userInput: out userInput,
                defaultValue: defaultValue,
                preValidators: preValidators,
                shouldRetry: shouldRetry,
                exitCondition: exitCondition,
                displayError: displayError
            );

            return errors;
        }

        public static IEnumerable<string> ReadInt(
            string prompt,
            IEnumerable<Func<int?, string?>> validators,
            out int? userInput,
            int? defaultValue = null,
            bool shouldRetry = true,
            IEnumerable<Func<string, string?>>? preValidators = null,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            var errors = ReadValue(
                prompt: prompt,
                parser: input => int.TryParse(input, out int val) ? val : throw new Exception(INVALID_FORMAT_ERROR),
                validators: validators,
                userInput: out userInput,
                defaultValue: defaultValue,
                preValidators: preValidators,
                shouldRetry: shouldRetry,
                exitCondition: exitCondition,
                displayError: displayError
            );

            return errors;
        }

        public static IEnumerable<string> ReadEnum<TEnum>(
            string prompt,
            IEnumerable<Func<TEnum?, string?>> validators,
            out TEnum? userInput,
            TEnum? defaultValue = null,
            bool shouldRetry = true,
            IEnumerable<Func<string, string?>>? preValidators = null,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        ) where TEnum : struct, Enum
        {
            var errors = ReadValue(
                prompt: prompt,
                parser: input =>
                {
                    // Trim and ignore case when parsing enum and handle int values
                    if (int.TryParse(input, out int intValue))
                    {
                        if (Enum.IsDefined(typeof(TEnum), intValue))
                        {
                            return (TEnum)Enum.ToObject(typeof(TEnum), intValue);
                        }
                    }

                    if (Enum.TryParse<TEnum>(input, true, out TEnum result))
                    {
                        return result;
                    }
                    throw new Exception(INVALID_FORMAT_ERROR);

                },
                validators: validators,
                userInput: out userInput,
                defaultValue: defaultValue,
                preValidators: preValidators,
                shouldRetry: shouldRetry,
                exitCondition: exitCondition,
                displayError: displayError
            );

            return errors;
        }

        public static IEnumerable<string> ReadDateTime(
            string prompt,
            IEnumerable<Func<DateTime?, string?>> validators,
            out DateTime? userInput,
            DateTime? defaultValue = null,
            bool shouldRetry = true,
             IEnumerable<Func<string, string?>>? preValidators = null,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            var errors = ReadValue(
                prompt: prompt,
                parser: input => DateTime.TryParse(input, out DateTime val) ? val : throw new Exception(INVALID_FORMAT_ERROR),
                validators: validators,
                userInput: out userInput,
                defaultValue: defaultValue,
                preValidators: preValidators,
                shouldRetry: shouldRetry,
                exitCondition: exitCondition,
                displayError: displayError
            );
            return errors;
        }

        public static IEnumerable<string> ReadConfirmation(
            string prompt,
            out bool userInput,
            bool defaultValue = false,
            bool shouldRetry = true,
            IEnumerable<Func<string, string?>>? preValidators = null,
            Func<string, bool>? exitCondition = null,
            Action<IEnumerable<string>>? displayError = null
        )
        {
            var errors = ReadValue<bool>(
                prompt: prompt,
                parser: input =>
                {
                    if (string.Equals(input, "y", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(input, "yes", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(input, "o", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(input, "oui", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(input, "no", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(input, "non", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    throw new Exception(INVALID_FORMAT_ERROR);
                },
                validators: [],
                userInput: out userInput,
                defaultValue: defaultValue,
                preValidators: preValidators,
                shouldRetry: shouldRetry,
                exitCondition: exitCondition,
                displayError: displayError
            );

            return errors;
        }
    }
}