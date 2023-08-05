
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TastyBytesReact.Repository;

namespace TastyBytesReact
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();

            var Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            builder.Services.AddSingleton<IConfiguration>(Configuration);
            builder.Services.AddSingleton<IJwtManagerRepository, JwtManagerRepository>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(Configuration.GetSection("JWT")["Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetSection("JWT")["Issuer"],
                    ValidAudience = Configuration.GetSection("JWT")["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime= true,
                    //On production set them to true 
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}