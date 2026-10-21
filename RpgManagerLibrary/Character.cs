
namespace RpgManagerLibrary
{
    public abstract class Character
    {
        private const int MinimumHealth = -100;
        public required string Name { get; set; }
        public required int PowerLevel { get; set; }
        public required Player Player { get; set; }
        public decimal Health { get; private set; } = 100;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public abstract string CharacterType { get; }

        protected Character()
        {
            
        }

        protected Character(int health)
        {
            Health = health;
        }

        /// <summary>
        /// Adds health to the character.
        /// </summary>
        /// <param name="amount">The amount of health to add. Must be positive.</param>
        public void Heal(decimal amount)
        {
            if (amount > 0)
            {
                Health += amount;
            }
        }

        /// <summary>
        /// Removes health from the character.
        /// </summary>
        /// <param name="amount">The amount of health to remove. Must be positive.</param>
        /// <exception cref="DamageTooHighException">If requested amount of damage would drop health below MinimumHealth.</exception>
        public void Damage(decimal amount)
        {
            if (amount > 0)
            {
                if (Health - amount < MinimumHealth)
                {
                    throw new DamageTooHighException($"Damage would subceed minimum health of character ({MinimumHealth})");
                }

                Health -= amount;
            }
        }


        public override string ToString()
        {
            // :N2 maakt dat Healt wordt geformatteerd als nummer en max 2 cijfers na de komma laat zien
            // cf.: https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#numeric-format-specifier-n
            return $"{Name} – {CharacterType} {Environment.NewLine}" +
                $"Health: {Health:N2} / PowerLevel: {PowerLevel} / Created: {CreationDate:d}";
        }
    }
}
