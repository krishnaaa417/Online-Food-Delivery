using ePizza.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Claims;

namespace ePizza.UI.Helpers
{
    public abstract class BasePageView<TModel>: RazorPage<TModel>
    {

        public UserModel CurrentUser
        {
            get
            {
                if (User.Claims.Count() > 0)
                {
                    string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                    string email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

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
