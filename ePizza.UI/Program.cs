using ePizza.UI.Helpers.TokenHelpers;
using ePizza.UI.RazorPay;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ePizza.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login";
                    options.LogoutPath = "/Login/Logout";
                });

            builder.Services.AddAuthorization();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<ITokenService,TokenService>();
            builder.Services.AddTransient<IRazorPayService, RazorPayService>();
            builder.Services.AddTransient<TokenHandler>();

            builder.Services.AddHttpClient("ePizaaApiClient", options =>
            {
                options.BaseAddress = new Uri(builder.Configuration["EPizzaApi:BaseAddress"]!);
                options.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<TokenHandler>();
        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
