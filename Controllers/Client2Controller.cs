using Microsoft.AspNetCore.Mvc;

namespace Koshelekpy_Test.Controllers
{
    public class Client2Controller : Controller
    {
        [HttpGet]
        [Route("/Client2")]
        public IActionResult Client2View()
        {
            return View();
        }
    }
}
