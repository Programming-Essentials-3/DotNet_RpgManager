using RpgManagerLibrary;

namespace RpgManagerConsole
{
    /// <summary>
    /// Klasse gebruikt om kadertjes te tekenen in de console.
    /// De code in deze klasse wordt aangeleverd 'as is' zonder uitgebreide uitleg, omdat het niet relevant is voor de kernfunctionaliteit van de applicatie.    
    /// </summary>
    static internal class ConsoleDraw
    {
        private static readonly BoxCharacters SingleLine = new()
        {
            Horizontal = '─',
            Vertical = '│',
            UpperLeft = '┌',
            UpperRight = '┐',
            LowerLeft = '└',
            LowerRight = '┘'
        };
        private static readonly BoxCharacters DoubleLine = new()
        {
            Horizontal = '═',
            Vertical = '║',
            UpperLeft = '╔',
            UpperRight = '╗',
            LowerLeft = '╚',
            LowerRight = '╝'
        };

        /// <summary>
        /// Tekent kadertje in de console met de opgegeven titel.
        /// De code in deze klasse is extra en wordt aangeleverd 'as is', zonder uitgebreide uitleg, 
        /// omdat het niet relevant is voor de kernfunctionaliteit van de applicatie.    
        /// </summary>
        internal static (int Left, int Top) DrawBox(string title, bool doubleLines = false, bool fullWidth = false, int padding = 7, ConsoleColor? textColour = null)
        {       
            int maxTitleLength = Console.WindowWidth - 2; // -2 voor de randen
            string[] lines = title.Split(Environment.NewLine, StringSplitOptions.TrimEntries);

            foreach (string line in lines)
            {
                if (line.Length > maxTitleLength - padding * 2)
                {
                    return (0, 0); // bij te lange titel, gewoon in stilte falen. todo: automatisch afbreken?
                    //throw new ArgumentException($"A title line exceeds maximum length of {MaxTitleLength} characters.", nameof(title));
                }
            }

            int maxLineLength = maxTitleLength;
            int totalWidth = maxLineLength;

            if (fullWidth)
            {
                padding = 0; // geen padding bij fullWidth
            }
            else
            {
                // bij multiline: maxLineLength = lengte van langste regel
                maxLineLength = lines.Length > 0 ? lines.Max(line => line.Length) : 0;
                totalWidth = maxLineLength + (padding * 2);
            }

            BoxCharacters chars = doubleLines ? DoubleLine : SingleLine;

            // bovenste lijn
            Console.Write(chars.UpperLeft);
            Console.Write(new string(chars.Horizontal, totalWidth));
            Console.WriteLine(chars.UpperRight);

            // inhoud
            foreach (string line in lines)
            {
                int leftPadding = padding + (maxLineLength - line.Length) / 2; // Center each line
                int rightPadding = padding + (maxLineLength - line.Length + 1) / 2; // Adjust for odd lengths
                Console.Write($"{chars.Vertical}{new string(' ', leftPadding)}");

                ConsoleColor currentColor = Console.ForegroundColor;

                if (textColour != null)
                {
                    Console.ForegroundColor = textColour.Value;
                }

                Console.Write(line);
                Console.ForegroundColor = currentColor;
                Console.WriteLine($"{new string(' ', rightPadding)}{chars.Vertical}");
            }

            // onderste lijn
            Console.Write(chars.LowerLeft);
            Console.Write(new string(chars.Horizontal, totalWidth));
            Console.WriteLine(chars.LowerRight);

            return Console.GetCursorPosition();
        }

        /// <summary>
        /// Print details (ToString) van het personage + speler bovenaan de console.
        /// De code in deze klasse is extra en wordt aangeleverd 'as is', zonder uitgebreide uitleg,
        /// omdat het niet relevant is voor de kernfunctionaliteit van de applicatie.
        /// </summary>
        internal static void WriteHeader(Character character, Player player)
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor textColour = ConsoleColor.White;

            // Beter zou zijn om Character een abstracte property Colour te geven,
            // maar omdat dit geen deel uitmaakt van de basisopdracht, houden we het hier bij "magic strings".
            if (character.CharacterType == "Mage")
            {
                textColour = ConsoleColor.Blue;
            }
            else if (character.CharacterType == "Warrior")
            {
                textColour = ConsoleColor.Red;
            }
            Console.ForegroundColor = ConsoleColor.White;
            (int Left, int Top) = DrawBox(character.ToString(), doubleLines: true, fullWidth: true, textColour: textColour);
            DrawTitle(Left, Top);
            DrawPlayerInfo(Left, Top, player);
        }

        private static void DrawTitle(int left, int top)
        {
            Console.SetCursorPosition(2, 0);
            Console.Write("╡");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("RPG Manager");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("╞");
            Console.ResetColor();
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// This is very hacky, ugly code, but it works for now.
        /// </summary>
        private static void DrawPlayerInfo(int left, int top, Player player)
        {
            Console.SetCursorPosition(left, top - 1);
            Console.ForegroundColor = ConsoleColor.White;
            (int NextLeft, int NextTop) = DrawBox($"Player: {player}", doubleLines: true, fullWidth: true);
            Console.SetCursorPosition(left, top - 1);
            Console.Write("╟");
            Console.Write(new string('─', Console.WindowWidth - 2));
            Console.SetCursorPosition(Console.WindowWidth - 1, top - 1);
            Console.WriteLine("╢");
            Console.SetCursorPosition(NextLeft, NextTop);
            Console.ResetColor();
        }
    }
}
