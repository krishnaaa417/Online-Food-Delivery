using ePizza.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ePizza.UI.Areas.Admin.Controllers
{
    // role based access controller
    [Area("Admin")]
    public class BaseController : Controller
    {
        public UserModel CurrentUser
        {
            get
            {
                if(User.Claims.Count() > 0)
                {
                    string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                    string email= User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

                    return new UserModel
                    {
                        Email = email,
                        Name = userName,
                    };
                }
                return null;
            }
        }
    }
}
