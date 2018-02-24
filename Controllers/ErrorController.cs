using Microsoft.AspNetCore.Mvc;

namespace WeddingPlanner.Controllers
{
    public class ErrorController : Controller {
        // public ViewResult Error() => View();
        [Route("/error")]
        public IActionResult Error() {
            return View("~/Views/Error/Error.cshtml");
        }
     }
}