namespace GameCore.Map
{
    public class Harita
    {
        public int MaxX;
        public int MaxY;
        public MapCellDecoder[,]? Hucreler { get; set; }

        public MapCellDecoder GetRandomLandCell(Random rnd)
        {
            MapCellDecoder hucre;
            do
            {
                var x = rnd.Next(0, MaxX - 1);
                var y = rnd.Next(0, MaxY - 1);
                hucre = Hucreler[x, y];

            } while (!hucre.CanUseAsStartup(rnd));
            return hucre;
        }

    }
}
