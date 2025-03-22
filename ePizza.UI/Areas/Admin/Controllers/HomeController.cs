using ePizza.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.UI.Areas.Admin.Controllers
{

   // [CustomAuthorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
