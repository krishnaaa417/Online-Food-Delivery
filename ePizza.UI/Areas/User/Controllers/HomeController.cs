using Microsoft.AspNetCore.Mvc;

namespace ePizza.UI.Areas.User.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
