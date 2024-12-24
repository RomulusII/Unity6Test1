using GameCore.Services;
using Microsoft.Extensions.Logging;
using Model.UnityOyun.Assets.Model;

namespace GameCore.Map
{
    public class MapService
    {
        private readonly ILogger _logger;

        public MapService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<List<MapCellBase>>> GetHucreler(int x, int y)
        {
            var rslt = new List<List<MapCellBase>>();
            try
            {
                var maxCell = 10;

                var maxX = Math.Min(x + maxCell, GameServiceStatic.GameService.Harita.MaxX);
                var minX = Math.Max(x - maxCell, 0);

                var maxY = Math.Min(y + maxCell, GameServiceStatic.GameService.Harita.MaxY);
                var minY = Math.Max(y - maxCell, 0);

                for (int i = minY; i < maxY; i++)
                {
                    var hucreLine = new List<MapCellBase>();
                    rslt.Add(hucreLine);

                    for (int j = minX; j < maxX; j++)
                    {
                        var hucre = GameServiceStatic.GameService.Harita.Hucreler?[j, i];
                        if (hucre == null)
                        {
                            _logger.LogWarning("Hucre null");
                            continue;
                        }
                        hucreLine.Add(hucre);
                    }
                }
                return rslt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hata");
                throw;
            }
        }
    }
}
