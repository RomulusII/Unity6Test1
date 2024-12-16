using GameCore.Services;
using System.Diagnostics;
using System.Drawing;

namespace GameCore.Map
{

    public class MapCellDecoder : MapCellBase
    {
        public string SeedFromBitmapUndefinedInfo { get; set; } = string.Empty;
        public Color SeedFromBitmapUndefinedColor { get; set; }
        public string Info => ToString();
        public MapCellDecoder(int x, int y, Color zemin, Color yukseklik, Color vejetasyon, Color nehir)
        {
            X = x;
            Y = y;
            Terrain = DecodeTerrain(zemin);
            Altitude = DecodeAltitude(yukseklik);
            Vegetation = DecodeVegetation(vejetasyon);
            Water = DecodeWater(nehir);
        }

        private void RenkTanimsiz(string tip, Color renk)
        {
            SeedFromBitmapUndefinedInfo = $"Tanimsiz {tip} koordinat ({X},{Y}), renk-Name: {renk.Name} renk-Argb: {renk.ToArgb() & 0xffffff} {renk}";
            SeedFromBitmapUndefinedColor = renk;
            Debug.WriteLine(SeedFromBitmapUndefinedInfo);
        }
        private CellTerrain DecodeTerrain(Color color)
        {
            switch ((uint)color.ToArgb())
            {
                case 0xff000000:
                    return CellTerrain.Deniz;
                case 0xffffff00:
                    return CellTerrain.Cöl;
                case 0xff00ffff:
                case 0xff818500:
                    return CellTerrain.Buzul;
                case 0xffff0000:
                    return CellTerrain.Bozkir;
                case 0xff0000ff:
                    return CellTerrain.Tundra;
                case 0xff008000:
                    return CellTerrain.KurakArazi;
                case 0xff00ff00:
                    return CellTerrain.Otlak;

                default:
                    RenkTanimsiz("Arazi", color);
                    return CellTerrain.NotSet;
            }
        }

        private CellAltitude DecodeAltitude(Color renk)
        {
            switch ((uint)renk.ToArgb())
            {
                case 0xffffffff:
                    return CellAltitude.Deniz;
                case 0xff000000:
                    return CellAltitude.Ova;
                //case 0xffff00ff:
                case 0xffff00ff:
                    return CellAltitude.Tepelik;
                case 0xff800000:
                case 0xff800080:
                    return CellAltitude.Daglik;
                default:
                    RenkTanimsiz("Yukseklik", renk);

                    return CellAltitude.NotSet;
            }
        }

        private CellVegetation DecodeVegetation(Color color)
        {
            switch ((uint)color.ToArgb())
            {
                case 0xff000000:
                case 0xffffffff:
                case 0xffc0c0c0:
                    return CellVegetation.Yok;
                case 0xff008000:
                case 0xff008080:
                    return CellVegetation.Orman;
                default:
                    RenkTanimsiz("Vejetasyon", color);
                    return CellVegetation.NotSet;
            }
        }

        private CellWater DecodeWater(Color renk)
        {
            switch ((uint)renk.ToArgb())
            {
                case 0xffffffff:
                case 0xff000000:
                    return CellWater.Yok;
                case 0xff0000ff:
                    return CellWater.Nehir;

                default:
                    RenkTanimsiz("Nehir", renk);
                    return CellWater.NotSet;
            }
        }

        private static List<CellTerrain> CanUseAsStartLandType => new() { CellTerrain.IslakArazi, CellTerrain.KurakArazi, CellTerrain.Bozkir, CellTerrain.Otlak };
        public bool CanUseAsStartup(Random rnd)
        {
            var quality = StartupTerrainTypeQuality();
            if (quality == 0) return false;

            var random = rnd.Next(1000);
            return quality > random;
        }

        private int StartupTerrainTypeQuality()
        {
            return Terrain switch
            {
                CellTerrain.Deniz or CellTerrain.Okyanus => 0,
                CellTerrain.Buzul => 1,
                CellTerrain.Tundra => 2,
                CellTerrain.Cöl => 5,
                CellTerrain.Bozkir => 20,
                CellTerrain.KurakArazi => 100,
                CellTerrain.Otlak => 1000,
                CellTerrain.IslakArazi => 1000,
                _ => throw new InvalidOperationException($"{nameof(CellTerrain)} {Terrain} not known."),
            };
        }
        public override string ToString()
        {
            var units = GameService.Game.GameContext.Units.Where(u => Math.Abs(u.X-X) + Math.Abs(u.Y - Y) < 10);
            var unitStr = string.Empty;
            foreach (var unit in units)
                {
                    if (unitStr != string.Empty) unitStr += ", ";
                    unitStr += unit.ToString();
                }

            return $"Hucre ({X}, {Y}) {Terrain}, {Altitude}, {Water}, {Vegetation}. {unitStr}";
        }
    }
}