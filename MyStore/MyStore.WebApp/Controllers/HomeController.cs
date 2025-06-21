using Microsoft.AspNetCore.Mvc;

namespace MyStore.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}