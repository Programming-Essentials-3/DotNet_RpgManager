namespace RpgManagerConsole
{
    /// <summary>
    /// Provides utility methods for reading and validating user input from the console.
    /// </summary>
    internal static class ConsoleInput
    {
        /// <summary>
        /// Print a menu with options and an exit option. Returns the chosen option as an integer.
        /// </summary>
        /// <param name="options">Options to display in the menu. Options are automatically numbered, starting with 1.</param>
        /// <param name="exitOption">Name for the exit option (always displayed on the bottom as option #0)</param>
        /// <returns>The index of the chosen option.</returns>
        public static int Menu(IList<string> options, string exitOption)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {options.ElementAt(i)}");
            }
            Console.WriteLine("0 - " + exitOption);

            return ReadInt("Choose an option: ", 0, options.Count);
        }

        /// <summary>
        /// Safely reads an integer from the console within the specified range.
        /// </summary>
        /// <remarks>If the user enters an unparsable input or an integer outside of the specified range, 
        /// an error message is displayed, and the prompt is repeated until valid input is provided.</remarks>
        /// <param name="prompt">Displayed before input.</param>
        /// <param name="min">Minimum valid value. Defaults to <see cref="int.MinValue"/>.</param>
        /// <param name="max">Maximum valid value. Defaults to <see cref="int.MaxValue"/>.</param>
        /// <returns>The integer entered by the user.</returns>
        public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue, int? defaultValue = null)
        {
            while (true)
            {
                string userInput = ReadFromConsole(prompt);
                if (string.IsNullOrWhiteSpace(userInput) && defaultValue != null)
                {
                    return defaultValue.Value;
                }

                if (!int.TryParse(userInput, out int input) || (input != defaultValue && (input < min || input > max)))
                {
                    ShowError("Invalid input!");
                    continue;
                }

                return input;
            }
        }

        /// <summary>
        /// Reads a non-empty string input from the console.
        /// </summary>
        /// <remarks>If the user enters an empty or whitespace-only input, an error message is displayed,
        /// and the prompt is repeated until valid input is provided.</remarks>
        /// <param name="prompt">The prompt message to display to the user.</param>
        /// <returns>The string entered by the user. The returned string is guaranteed to be non-null and non-whitespace.</returns>
        public static string ReadString(string prompt)
        {
            string? consoleInput;

            while (string.IsNullOrWhiteSpace(consoleInput = ReadFromConsole(prompt)))
            {
                ShowError("Invalid input!");
            }

            return consoleInput;
        }

        /// <summary>
        /// Reads a single character from the console after displaying the specified prompt.
        /// </summary>
        /// <param name="prompt">The message to display to the user before reading input.</param>
        /// <returns>The first character of the key entered by the user.</returns>
        public static char ReadChar(string prompt)
        {
            return ReadFromConsole(prompt, readKey: true)[0];
        }

        /// <summary>
        /// Reads input (string or char) from the console after displaying a prompt.
        /// </summary>
        /// <param name="prompt">The message to display to the user before reading input.</param>
        /// <param name="inputColor">The color to use for the input text. Defaults to <see cref="ConsoleColor.White"/>.</param>
        /// <param name="readKey">A value indicating whether to read a single key press (<see langword="true"/>) or a full line of text (<see
        /// langword="false"/>). Defaults to <see langword="false"/>.</param>
        /// <returns>The input provided by the user. If <paramref name="readKey"/> is <see langword="true"/>, the name of the key
        /// pressed is returned. Otherwise, the full line of text entered by the user is returned. If no input is
        /// provided, an empty string is returned.</returns>
        private static string ReadFromConsole(string prompt, ConsoleColor inputColor = ConsoleColor.White, bool readKey = false)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.Write(prompt);
            Console.ForegroundColor = inputColor;

            string? output;
            if (readKey)
            {
                output = Console.ReadKey().Key.ToString();
            }
            else
            {
                output = Console.ReadLine() ?? "";
            }

            Console.ForegroundColor = currentColor;
            return output;
        }

        /// <summary>
        /// Displays an error message in a specified color.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="errorColour">The <see cref="ConsoleColor"/> to display the message in (defaults to <see cref="ConsoleColor.DarkRed"/>.</param>
        public static void ShowError(string message, ConsoleColor errorColour = ConsoleColor.DarkRed)
        {
            Console.ForegroundColor = errorColour;
            Console.WriteLine(message);
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
