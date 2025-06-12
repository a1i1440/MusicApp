
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Music.API.Data;
using MusicApp.Music.API.Services;
using System.Text;

namespace MusicApp.Music.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(builder =>
            {
                builder.AddPolicy("AllowAllOrigins",
                     policy => policy.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader());
            });
            var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("Default");
            Console.WriteLine($"Using connection string: {connectionString}");
            builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));
            builder.Services.AddScoped<IFileService, FileService>();
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:SecretKey"]!);

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Console.WriteLine("Challenge event triggered");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 }
                    };

                });
            builder.Services.AddAuthorization();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
