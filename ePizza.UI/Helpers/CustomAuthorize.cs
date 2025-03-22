using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ePizza.UI.Helpers
{
    public class CustomAuthorize : Attribute ,IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if(!context.HttpContext.User.IsInRole(Roles))
                {
                    context.Result = new RedirectToActionResult("", "", new { area=""});
                }
            }
            else
            {
                // send user to login page
            }
        }
     
    }
}
