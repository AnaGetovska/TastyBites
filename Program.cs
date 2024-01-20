
using ArangoDBNetStandard;
using ArangoDBNetStandard.Transport.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TastyBytesReact.Repository;
using TastyBytesReact.Repository.Arango;
using TastyBytesReact.Services;
using TastyBytesReact.Utilities;

namespace TastyBytesReact
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();

            var Configuration = new ConfigurationBuilder().AddEnvironmentVariables("TB").AddJsonFile("appsettings.json").Build();
            builder.Services.AddSingleton<IConfiguration>(Configuration);

            var arangoConfig = Configuration.GetRequiredSection("ArangoDb");
            var transport = HttpApiTransport.UsingBasicAuth(
                new Uri(arangoConfig["URI"]),
                arangoConfig["Database"],
                arangoConfig["Username"],
                arangoConfig["Password"]);
            
            builder.Services.AddSingleton<IArangoDBClient>(new ArangoDBClient(transport));
            builder.Services.AddSingleton<IUserService, UserService>();


            #region Repos
            builder.Services.AddSingleton<IJwtManagerRepo, JwtManagerRepo>();
            builder.Services.AddSingleton<CategoryRepo>();
            builder.Services.AddSingleton<RecipeRepo>();
            builder.Services.AddSingleton<IngredientRepo>();
            builder.Services.AddSingleton<UserRepo>();
            #endregion

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
                    ValidateLifetime = true,
                    //On production set them to true 
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddCors(c => c.AddPolicy("corsApp", builder =>
               builder.WithOrigins("*").WithMethods("GET", "POST", "DELETE", "PUT").AllowAnyMethod().AllowAnyHeader()
            ));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("corsApp");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}