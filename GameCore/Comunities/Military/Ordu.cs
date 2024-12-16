using Model;

namespace GameCore.Communities.Asker
{
    public class Ordu : Unit
    {
        public Dictionary<AskerTip, Birlik> Birlikler { get; }

        public Ordu(Player player, int x, int y) : base(player, x, y)
        {
            UnitType = UnitType.Soldier;

            Birlikler = new Dictionary<AskerTip, Birlik>();

            foreach (AskerTip t in Enum.GetValues(typeof(AskerTip)))
            {
                Birlikler.Add(t, new Birlik(t));
            }
        }


    }
}
