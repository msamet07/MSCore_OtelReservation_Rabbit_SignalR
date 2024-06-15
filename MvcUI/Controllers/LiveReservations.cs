using Microsoft.AspNetCore.Mvc;

namespace MvcUI.Controllers
{
    public class LiveReservations : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
