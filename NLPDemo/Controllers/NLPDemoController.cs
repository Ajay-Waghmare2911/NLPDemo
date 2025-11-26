using Microsoft.AspNetCore.Mvc;

namespace NLPDemo.Controllers
{
    public class NLPDemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
