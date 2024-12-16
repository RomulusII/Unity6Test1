using Microsoft.AspNetCore.Mvc;

namespace UnityApi.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
