using GameCore.Map;
using GameCore.Services;
using Microsoft.AspNetCore.Mvc;
using Model.UnityOyun.Assets.Model;

namespace OyunApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OyunController : ControllerBase
    {

        private readonly ILogger<OyunController> _logger;

        public OyunController(ILogger<OyunController> logger)
        {
            _logger = logger;
        }

        [HttpGet("hucre")]
        public ActionResult<MapCellDecoder> Get(int x, int y)
        {
            try
            {
                var cell = GameService.Game.Harita.Hucreler[x, y];
                return Ok(cell);

            }
            catch(IndexOutOfRangeException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }        
        }

        [HttpGet("hucreler")]
        public ActionResult<List<List<MapCellBase>>> GetHucreler(int x, int y)
        {
            try
            {
                var maxCell = 10;

                var rslt = new List<List<MapCellBase>>();

                var maxX = Math.Min(x + maxCell, GameService.Game.Harita.MaxX);
                var minX = Math.Max(x - maxCell, 0);

                var maxY = Math.Min(y + maxCell, GameService.Game.Harita.MaxY);
                var minY = Math.Max(y - maxCell, 0);

                for(int i = minY; i < maxY; i++)
                {
                    var hucreLine = new List<MapCellBase>();
                    rslt.Add(hucreLine);

                    for (int j = minX; j < maxX; j++)
                    {

                        var hucre = GameService.Game.Harita.Hucreler[j, i];
                        hucreLine.Add(hucre);
                    }
                }
                return Ok(rslt);

            }
            catch (IndexOutOfRangeException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}