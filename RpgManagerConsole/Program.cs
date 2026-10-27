using RpgManagerLibrary;

namespace RpgManagerConsole
{
    internal class Program
    {
        private static Player CurrentPlayer = new() { FirstName = "Wim", LastName = "Hambrouck" };
        private static Character CurrentCharacter = new Mage() { Name = "William", PowerLevel = 9999, ManaBoost = 10, Player = CurrentPlayer };

        private static readonly List<Character> Characters = [CurrentCharacter];
        private static readonly List<Player> Players = [CurrentPlayer];

        static void Main(string[] args)
        {
            Warrior warrior = new(health: 150) { Name = "Zarkan the DESTROYER!", PowerLevel = 100, Player = CurrentPlayer };
            warrior.Weapons.Add("Sting");
            warrior.Weapons.Add("Sword of a Thousand Truths");
            warrior.Weapons.Add("Aperture Science Handheld Portal Device");
            Characters.Add(warrior);

            Player player2 = new() { FirstName = "Anna", LastName = "Smith" };
            Players.Add(player2);

            bool continueApp = true;
            while (continueApp)
            {
                Console.Clear();
                ConsoleDraw.WriteHeader(CurrentCharacter, CurrentPlayer);

                ConsoleDraw.DrawBox("Menu");
                int keuze = ConsoleInput.Menu(
                    [
                      "Heal",
                      "Damage",
                      "New Character",
                      "Switch Character",
                      "New Player",
                      "Switch Player"
                    ],
                    exitOption: "Quit"
                    );

                switch (keuze)
                {
                    case 1:
                        HealCharacter();
                        break;
                    case 2:
                        DamageCharacter();
                        break;
                    case 3:
                        AddCharacterForPlayer(CurrentPlayer);
                        break;
                    case 4:
                        SwitchCharacter();
                        break;
                    case 5:
                        AddPlayer();
                        break;
                    case 6:
                        SwitchPlayer();
                        break;
                    default:
                        continueApp = false;
                        break;
                }
            }

            Console.ResetColor();
            Console.WriteLine("Bye!");
        }

        private static void HealCharacter()
        {
            int healAmount = ConsoleInput.ReadInt("Enter amount to heal (1 - 1000, 0 to cancel) [0]: ", 1, 1000, 0);
            CurrentCharacter.Heal(healAmount);
        }

        private static void DamageCharacter()
        {
            bool continueLoop = true;

            while (continueLoop)
            {
                int damageAmount = ConsoleInput.ReadInt("Enter amount of damage (1 - 1000, 0 to cancel) [0]: ", 1, 1000, 0);
                try
                {
                    CurrentCharacter.Damage(damageAmount);
                    continueLoop = false;
                }
                catch (DamageTooHighException e)
                {
                    ConsoleInput.ShowError(e.Message);
                    char retry = ConsoleInput.ReadChar("Do you want to try again? (Y/N) [N]: ");
                    Console.WriteLine();
                    continueLoop = retry == 'Y' || retry == 'y';
                }
            }
        }

        private static bool AddCharacterForPlayer(Player player)
        {
            ConsoleDraw.DrawBox("Create new character");
            // keuzes zijn hier hardcoded, zou op basis van enum kunnen of met een beheerbare lijst in een databank
            switch (ConsoleInput.Menu(["Mage", "Warrior"], "Cancel"))
            {
                case 1:
                    NewMage(player);
                    return true;
                case 2:
                    NewWarrior(player);
                    return true;
                default:
                    return false;
            }
        }

        private static void NewWarrior(Player player)
        {
            string warriorName = ConsoleInput.ReadString("What should this fearsome warrior be called? ");
            Warrior warrior = new() { Name = warriorName, PowerLevel = 9999, Player = player };

            string weapon = ConsoleInput.ReadString("Add weapon (type weapon name or Q to quit): ");
            while (!weapon.Equals("Q", StringComparison.CurrentCultureIgnoreCase))
            {
                warrior.Weapons.Add(weapon);
                weapon = ConsoleInput.ReadString("Add weapon: ");
            }
            CurrentCharacter = warrior;
            Characters.Add(CurrentCharacter);
        }

        private static void NewMage(Player player)
        {
            string mageName = ConsoleInput.ReadString("What should this mystical mage be called? ");
            CurrentCharacter = new Mage() { Name = mageName, PowerLevel = 9999, ManaBoost = 10, Player = player };
            Characters.Add(CurrentCharacter);
        }

        private static void SwitchCharacter()
        {
            Console.Clear();
            ConsoleDraw.WriteHeader(CurrentCharacter, CurrentPlayer);

            ConsoleDraw.DrawBox("Switch Character", fullWidth: true);
            ConsoleDraw.DrawBox("Select a character to switch to", padding: 1);

            // Ter info: volgende code kan gemakkelijker met LINQ, maar dat is op dit punt in de cursus nog niet behandeld.
            // Met LINQ kan de rest van deze methode vervangen volgen met deze code:
            //      IEnumerable<Character> list = Characters.Where(c => c.Player == CurrentPlayer);
            //      int keuze = ConsoleInput.Menu([.. list.Select(c => $"{c.Name} ({c.CharacterType})")], "Cancel");
            //      if (keuze != 0)
            //      {
            //          CurrentCharacter = list.ElementAt(keuze - 1);
            //      }
            Dictionary<Character, string> playerCharacters = [];
            foreach (Character character in Characters)
            {
                // enkel personages van de huidige speler tonen
                // Noot: we willen effectief de reference vergelijken, dat is sneller en
                // speler mag immers meerdere personages hebben met dezelfde naam
                if (character.Player == CurrentPlayer)
                {
                    playerCharacters.Add(character, $"{character.Name} ({character.CharacterType})");
                }
            }

            int keuze = ConsoleInput.Menu([.. playerCharacters.Values], "Cancel");

            // OPGELET: De Menu-methode controleert op juiste invoer.
            //
            // Als je rechtstreeks invoer van de Console zou lezen,
            // dan zou je hier extra validatie moeten toevoegen (keuze >=1 && keuze <= Characters.Count)
            // om ervoor te zorgen dat je niet buiten de grenzen van de lijst gaat.
            if (keuze != 0)
            {
                CurrentCharacter = playerCharacters.ElementAt(keuze - 1).Key;
            }
        }

        private static void AddPlayer()
        {
            Player newPlayer = new()
            {
                FirstName = ConsoleInput.ReadString("Enter first name: "),
                LastName = ConsoleInput.ReadString("Enter last name: ")
            };

            if (AddCharacterForPlayer(newPlayer))
            {
                Players.Add(newPlayer);
                CurrentPlayer = newPlayer;
            }
        }

        private static void SwitchPlayer()
        {
            // same as SwitchCharacter but for players
            Console.Clear();
            ConsoleDraw.WriteHeader(CurrentCharacter, CurrentPlayer);
            ConsoleDraw.DrawBox("Switch Player", fullWidth: true);
            ConsoleDraw.DrawBox("Select player to switch to", padding: 1);
            List<string> playerNames = [];
            foreach (Player player in Players)
            {
                playerNames.Add(player.ToString());
            }
            int keuze = ConsoleInput.Menu(playerNames, "Cancel");

            // OPGLET: De Menu-methode controleert op juiste invoer.
            //
            // Als je rechtstreeks invoer van de Console zou lezen,
            // dan zou je hier extra validatie moeten toevoegen (keuze >=1 && keuze <= PLayers.Count)
            // om ervoor te zorgen dat je niet buiten de grenzen van de lijst gaat.
            if (keuze != 0)
            {
                Player otherPlayer = Players[keuze - 1];

                // eerste personage van de gekozen speler zoeken
                // Noot: dit kan efficiënter met LINQ, maar dat is op dit punt in de cursus nog niet behandeld.
                foreach (Character character in Characters)
                {
                    if (character.Player == otherPlayer)
                    {
                        CurrentCharacter = character;
                        break;
                    }
                }

                if (CurrentCharacter.Player == otherPlayer)
                {
                    CurrentPlayer = otherPlayer;
                }
                else
                {
                    ConsoleInput.ShowError("The selected player has no characters yet. Please create a new character for this player first.");
                    //Console.Write("Press <ENTER> to continue with current player...");
                    //Console.ReadLine();

                    char invoer = ConsoleInput.ReadChar("Create new character for this player? ([Y]es/[N]o) [N] ");
                    if (invoer == 'Y')
                    {
                        Console.WriteLine();
                        if (AddCharacterForPlayer(otherPlayer))
                        {
                            CurrentPlayer = otherPlayer;
                        }
                    }
                }
            }
        }
    }
}
