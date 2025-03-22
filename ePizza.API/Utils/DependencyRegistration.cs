using ePizza.Core.Concrete;
using ePizza.Core.Contracts;
using ePizza.Core.Utils;
using ePizza.Domain.Models;
using ePizza.Repository.Concrete;
using ePizza.Repository.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ePizza.API.Utils
{
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {

            services.AddSingleton<TokenGenerator>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IOrderService, OrderService>();



            return services;
        }

        public static IServiceCollection RegisterDbDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<EpizzaHubDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
            });


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolesRepository, RoleRepository>();
            services.AddScoped<IItemRespository, ItemRespository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        public static IServiceCollection RegisterJwt(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                       ValidIssuer = configuration["Jwt:Issuer"],
                       ValidAudience = configuration["Jwt:Audience"],
                       ClockSkew = TimeSpan.Zero
                   };
               });

            return services;
        }

    }
}
