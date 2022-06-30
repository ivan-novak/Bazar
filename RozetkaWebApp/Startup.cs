//MLHIDEFILE
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RozetkaWebApp.Data;
using System.Security.Claims;

namespace RozetkaWebApp
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;

            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/google-login";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "1247207429440769";
                    options.AppSecret = "80cfe74ca807966f174fd9c86a9c4f4b";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = "363893018744-v3iue25f54a38iu96q1uiqctiag9asnu.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });

            services.AddCors();
            services.AddDbContext<RozetkadbContext>(options =>
                options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();


            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();
            services.AddControllersWithViews();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Owner", policy =>
                    policy.RequireAssertion(context =>
                    {
                        if (context.Resource is HttpContext httpContext)
                        {
                            object Id = "";
                            if (!httpContext.Request.RouteValues.TryGetValue("Id", out Id)) return false;
                            var User = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                            if (User == null) return false;
                            var Id1 = User.Value;
                            return Id.ToString() == Id1.ToString();
                        }
                        return false;
                    }));

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
