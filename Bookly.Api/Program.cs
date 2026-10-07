
using Bookly.Api.Middlewares;
using Bookly.Application.DependancyInjection;
using Bookly.Infrastructure.DependancyInjection;
using System.Reflection;

namespace Bookly.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicaiton();
            builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);
            builder.Services.AddTransient<ErrorHandlingMiddleware>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(
                    AppContext.BaseDirectory,
                    xmlFile);

                options.IncludeXmlComments(xmlPath);
            });
            var app = builder.Build();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
