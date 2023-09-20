using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class GPSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
