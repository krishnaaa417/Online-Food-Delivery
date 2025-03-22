using ePizza.API.Middlewares;
using ePizza.API.Utils;
using Serilog;
using Serilog.Exceptions;


namespace ePizza.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                   .Enrich.WithMachineName()
                      .Enrich.WithEnvironmentName()
                   .Enrich.WithExceptionDetails()
                 .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.RegisterDbDependencies(builder.Configuration)
                .RegisterServices()
                .RegisterJwt(builder.Configuration);



            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseSerilogRequestLogging();

            app.UseMiddleware<CommonResponseMiddleware>();  // registering a middleware

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
