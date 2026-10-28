namespace RpgManagerLibrary
{
    public class Warrior : Character
    {
        public List<string> Weapons { get; set; } = [];

        public override string CharacterType => "Warrior";

        public Warrior()
        {
            
        }

        public Warrior(int health) : base(health)
        {
        }

        override public string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}Weapons: {string.Join(", ", Weapons)}";
        }
    }
}
