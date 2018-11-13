using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertOne.Web.Controllers
{
    public class ApplicationController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
