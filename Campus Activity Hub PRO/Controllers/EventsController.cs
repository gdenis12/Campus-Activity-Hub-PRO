using Microsoft.AspNetCore.Mvc;

namespace Campus_Activity_Hub_PRO.Controllers
{
    public class EventsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
