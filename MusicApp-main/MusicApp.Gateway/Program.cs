
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace MusicApp.Gateway
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://*:8000");
            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOcelot();

            builder.Services.AddCors(builder =>
            {
                builder.AddPolicy("AllowAllOrigins",
                     policy => policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                                     .AllowAnyMethod()
                                     .AllowAnyHeader());
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseAuthorization();
            await app.UseOcelot();
            app.Run();
        }
    }
}
