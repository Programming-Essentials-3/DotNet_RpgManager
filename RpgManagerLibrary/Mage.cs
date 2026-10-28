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

        override public string ToString()
        {
            return $"{base.ToString()} / ManaBoost: {ManaBoost}";
        }
    }
}
