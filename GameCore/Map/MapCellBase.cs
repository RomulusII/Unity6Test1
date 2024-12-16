using Model;

namespace GameCore.Map
{
    public class MapCellBase : CoordinateBase
    {
        public CellTerrain Terrain { get; set; }
        public CellAltitude Altitude { get; set; }
        public CellWater Water { get; set; }
        public CellVegetation Vegetation { get; set; }
    }
}