using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZenithWebsite.Data;
using ZenithWebsite.Models;
using ZenithWebsite.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SimpleTokenProvider;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Cors.Infrastructure;
using OpenIddict;
using CryptoHelper;

namespace ZenithWebsite
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                //* This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //enable cors
            services.AddCors(options => options.AddPolicy("AllowAll",
                                            p => p.AllowAnyOrigin()
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()
                                                .AllowCredentials()));


            services.AddOpenIddict<ApplicationDbContext>()

                        .AddMvcBinders()
                        .EnableAuthorizationEndpoint("/connect/authorize")
                        .EnableTokenEndpoint("/connect/token")
                        .EnableLogoutEndpoint("/connect/logout")
                        .AllowPasswordFlow()
                        .AllowAuthorizationCodeFlow()
                        .DisableHttpsRequirement()
                        .AddEphemeralSigningKey();



            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Database 
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ZenithContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddEntityFrameworkStores<ZenithContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Cookie settings
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";

                // User settings
                options.User.RequireUniqueEmail = true;


            });

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            ZenithContext db, ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            //Use the new policy globally

            app.UseCors("AllowAll");

            app.UseOAuthValidation();

            app.UseOpenIddict();


            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvcWithDefaultRoute();


            using ( context = new ApplicationDbContext(
    app.ApplicationServices.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                if (!context.Applications.Any())
                {
                    context.Applications.Add(new OpenIddictApplication
                    {
                        // Assign a unique identifier to your client app:
                        Id = "48BF1BC3-CE01-4787-BBF2-0426EAD21342",

                        // Assign a display named used in the consent form page:
                        DisplayName = "MVC Core client application",

                        // Register the appropriate redirect_uri and post_logout_redirect_uri:
                        RedirectUri = "http://localhost:53507/signin-oidc",
                        LogoutRedirectUri = "http://localhost:53507/",
                        ClientSecret = Crypto.HashPassword("secret_secret_secret"),

                        // Note: use "public" for JS/mobile/desktop applications
                        // and "confidential" for server-side applications.
                        Type = OpenIddictConstants.ClientTypes.Confidential
                    });

                    context.SaveChanges();
                }
            }

            //TRYING WEB API AUTH
            // secretKey contains a secret passphrase only your server knows
            string secretKey = "mysupersecret_secretkey!123";
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ExampleIssuer",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "ExampleAudience",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            //TOKEN
            // Add JWT generation endpoint:
            var options = new TokenProviderOptions
            {
                Audience = "ExampleAudience",
                Issuer = "ExampleIssuer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));


            //END OF AUTH TRIAL

            // Seed demo Activities and Events if they don't exist 
            ZenithSeeder.Seed(db);

            // Create admin and member roles and accounts if they don't exist
            createRolesandUsers(context, roleManager, userManager);
        }

        // In this method we will create default User roles and Admin user for login   
        private async void createRolesandUsers(
            ApplicationDbContext context, 
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            // creating Creating Member role    
            if (!await roleManager.RoleExistsAsync("Member"))
            {
                var role = new IdentityRole();
                role.Name = "Member";
                var roleResult = await roleManager.CreateAsync(role);

                // Here we create a Admin super user who will maintain the website                  
                var user = new ApplicationUser();
                user.UserName = "m";
                user.Email = "m@m.c";
                string userPWD = "P@$$w0rd";

                var chkUser = await userManager.CreateAsync(user, userPWD);

                if (chkUser.Succeeded)
                {
                    var result1 = await userManager.AddToRoleAsync(user, "Member");
                }
            }

            // Create first Admin Role and creating a default Admin User   
            var adminExists = await roleManager.RoleExistsAsync("Admin");
            if (!adminExists)
            {
                // first we create Admin role
                var role = new IdentityRole();
                role.Name = "Admin";
                var roleResult = await roleManager.CreateAsync(role);

                // Here we create a Admin super user who will maintain the website                  
                var user = new ApplicationUser();
                user.UserName = "ZenithAdmin";
                user.Email = "admin@zenith.com";
                string userPWD = "!@#123QWEqwe";
                // Create an admin user for marking 
                var user2 = new ApplicationUser();
                user2.UserName = "a";
                user2.Email = "a@a.a";
                string user2PWD = "P@$$w0rd";

                //Add default User to Role Admin  
                var chkUser = await userManager.CreateAsync(user, userPWD);
                var chkUser2 = await userManager.CreateAsync(user2, user2PWD);
                if (chkUser.Succeeded)
                {
                    var result1 = await userManager.AddToRolesAsync(user, new string[] { "Admin", "Member"});
                    var result2 = await userManager.AddToRoleAsync(user2, "Admin");
                }
            }
        }

        
    }
}
