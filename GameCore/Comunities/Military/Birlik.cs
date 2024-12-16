namespace GameCore.Communities.Asker
{
    public class Birlik
    {
        public AskerTip AskerTipi;

        public int Adet { get; set; }

        public Birlik(AskerTip askerTipi)
        {
            AskerTipi = askerTipi;
            Adet = 0;
        }
    }
}