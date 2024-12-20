using GameCore.Map;
using GameCore.Services;
using Model;

namespace GameCore.Extensions
{
    public static class Extensions
    {
        public static MapCellDecoder? GetHucre(this Unit unit)
        {
            return GameServiceStatic.GameService?.Harita?.Hucreler?[unit.X, unit.Y];
        }
    }

}
