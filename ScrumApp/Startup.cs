using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrumApp.Data;
using ScrumApp.Hubs;
using ScrumApp.Models;
using ScrumApp.Services;
using ScrumApp.Services.Board_;
using ScrumApp.Services.BoardColumn_;
using ScrumApp.Services.Invitation_;
using ScrumApp.Services.ProjectS;

namespace ScrumApp
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



            services.AddControllersWithViews();
                

            
            services.AddSignalR();
            services.AddDbContext<ScrumApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScrumApplicationContext")));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ScrumApplicationContext>()
                .AddDefaultTokenProviders();



            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IInvitationService, InvitationService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IBoardColumnService, BoardColumnService>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapHub<BoardHub>("/boardhub");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default2",
                     pattern: "{userSlug}/{projectSlug}/{boardSlug}/{controller}/{action}",
                    //pattern: "{userSlug}/{projectSlug}/{boardSlug}/{controller}/{action}",
                    defaults: new { controller = "Board", action = "Board" }
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{userSlug}/{projectSlug}/{controller}/{action}",
                    defaults: new { controller = "Board", action = "Index" }
                );

                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Projects}/{action=Index}/{id?}",
                        defaults: new { controller = "Projects" }
                );



                /*
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{userSlug}/{projectSlug}/{controller}/{boardSlug}",
                        defaults: new { controller = "Board", action = "test" }
                );
                */




            });
            CreateAdminRole(serviceProvider);
            CreateAdminUserAsync(serviceProvider);
        }


        private void CreateAdminRole(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Standard" };

            foreach (string role in roles)
            {
                Task<bool> roleExists = roleManager.RoleExistsAsync(role);
                roleExists.Wait();

                if (!roleExists.Result)
                {
                    Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(role));
                    roleResult.Wait();
                }
            }
        }

        public void CreateAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            AppUser admin = new AppUser
            {
                Email = "williamganrot@hotmail.com",
                UserName = "admin",
                UserNameSlug = "admin"
            };

            string pass = "Admin_1";

            Task<IdentityResult> newUser = userManager.CreateAsync(admin, pass);
            newUser.Wait();

            if (newUser.Result.Succeeded)
            {
                Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(admin, "Admin");
                newUserRole.Wait();
            }

        }
    }
}