using Microsoft.AspNetCore.Mvc;

namespace Koshelekpy_Test.Controllers
{
    public class Client3Controller : Controller
    {
        [HttpGet]
        [Route("/Client3")]
        public IActionResult Client3View()
        {
            return View();
        }
    }
}
