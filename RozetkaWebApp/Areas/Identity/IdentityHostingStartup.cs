//using System;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using RozetkaWebApp.Data;

//[assembly: HostingStartup(typeof(RozetkaWebApp.Areas.Identity.IdentityHostingStartup))]
//namespace RozetkaWebApp.Areas.Identity
//{
//    public class IdentityHostingStartup : IHostingStartup
//    {
//        public void Configure(IWebHostBuilder builder)
//        {
//            builder.ConfigureServices((context, services) => {
//                services.AddDbContext<RozetkaWebAppContext>(options =>
//                    options.UseSqlServer(
//                        context.Configuration.GetConnectionString("RozetkaWebAppContextConnection")));

//                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//                    .AddEntityFrameworkStores<RozetkaWebAppContext>();
//            });
//        }
//    }
//}