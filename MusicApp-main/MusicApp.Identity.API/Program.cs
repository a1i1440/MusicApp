
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Identity.API.Data;
using MusicApp.Identity.API.Data.Entities;
using MusicApp.Identity.API.Services;
using System.Text;

namespace MusicApp.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("Default");
            Console.WriteLine($"Connection String: {connectionString}");
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddCors(builder =>
            {
                builder.AddPolicy("AllowAllOrigins",
                     policy => policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                                     .AllowAnyMethod()
                                     .AllowAnyHeader());
            });
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
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                Console.WriteLine("Inside scope");
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                try
                {
                    Console.WriteLine("Applying migrations...");
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Migration failed: " + ex.Message);
                }

            }
            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseAuthorization();
            app.UseAuthorization();

            app.MapControllers();
            Console.WriteLine("Starting the app");
            app.Run();
        }
    }
}
