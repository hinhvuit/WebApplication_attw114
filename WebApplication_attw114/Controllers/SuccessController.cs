using Microsoft.AspNetCore.Mvc;

namespace WebApplication_attw114.Controllers
{
    public class SuccessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Sign() { return View(); }
    }
}
