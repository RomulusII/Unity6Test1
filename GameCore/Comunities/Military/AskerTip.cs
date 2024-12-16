namespace GameCore.Communities.Asker
{
    public enum AskerTip
    {
        Kargi, Kilicli, Baltaci, Okcu, HafifSüvari, AgirSüvari,
    }

    public class AskerÖzellikleri
    {
        public string Isim { get; } = default!;
        public double Hiz { get; }

    }

}