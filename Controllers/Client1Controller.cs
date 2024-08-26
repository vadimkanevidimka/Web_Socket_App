using Microsoft.AspNetCore.Mvc;

namespace Koshelekpy_Test.Controllers
{
    public class Client1Controller : Controller
    {
        [HttpGet]
        [Route("/Client1")]
        public IActionResult Client1View()
        {
            return View();
        }
    }
}
