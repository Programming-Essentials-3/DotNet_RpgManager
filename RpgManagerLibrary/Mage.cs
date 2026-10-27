namespace RpgManagerLibrary
{
    public class Mage : Character
    {
        public int ManaBoost { get; set; }

        public override string CharacterType => "Mage";


        public Mage()
        {
            
        }

        public Mage(int health) : base(health)
        {
            
        }

        //public Mage(string name, decimal health, DateTime creationDate, int powerLevel, int manaBoost, Player player) : base(name, health, player, powerLevel, creationDate)
        //{
        //    ManaBoost = manaBoost;
        //}

        override public string ToString()
        {
            return $"{base.ToString()} / ManaBoost: {ManaBoost}";
        }
    }
}
