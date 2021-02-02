using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FirstWebApp.Models;
using FirstWebApp.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FirstWebApp
{
    public class Startup
    {
        private IConfiguration _config;
        /// <summary>
        /// We stored connection string in appsettigs.json file and to read that connection string value
        /// we are using this IConfiguration service
        /// </summary>
        
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        /// <summary>
        /// Below is the dependency injection container. 
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Tokens.EmailConfirmationTokenProvider = "CustomerEmailConfirmation";
            }).AddEntityFrameworkStores<AppDbContext>()

            //to add default tokens for resreting pswds and email cahnge
                .AddDefaultTokenProviders()
                .AddTokenProvider<CustomEmailTokenProvider<ApplicationUser>>("CustomerEmailConfirmation");
            services.Configure<DataProtectionTokenProviderOptions>(op => op.TokenLifespan = TimeSpan.FromHours(5));
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(op => op.TokenLifespan = TimeSpan.FromDays(3));
            
            //services.Configure<IdentityOptions>(options => options.Password.RequiredLength = 8);
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .Build();
                config.Filters.Add(new AuthorizeFilter(policy));                      
            });

            services.AddAuthentication()
                .AddGoogle(op => 
                    {
                        op.ClientId = "371109177633-rdsr8m5amprrgdpdpraghholtp6bhone.apps.googleusercontent.com";
                        op.ClientSecret = "kewuBsCFOuccrMKKfquTv7VF";
                    })
                .AddFacebook(op => 
                {
                    op.ClientId = "249683872957619";
                    op.ClientSecret = "670c4bc39fe3116124182d258b512a78";
                });
            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = new PathString("/Administration/AccessDenied"));
            //services.AddAuthorization(options => options.AddPolicy("EditRolePolicy", 
            //    policy => policy.RequireClaim("EditRole", "true")));
            services.AddAuthorization(options => 
                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesandClaimsRequirement()))
                );
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>(); //for sql server
            //services.AddTransient<IEmployeeRepository, IEmployeeRepository>(); //for in-memory collection 
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseStatusCodePages();
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseMvcWithDefaultRoute(); //Which is used for default routing. no need of explicit routing req.
            app.UseMvc(route =>
            {
                route.MapRoute("Default", "{controller=Home}/{action=index}/{id?}");
            }); //Conventional Routing
                //app.UseMvc();




            //app.Run(async (context) =>
            //{
            //    //throw new Exception("error");
            //    //await context.Response.WriteAsync("Hosting Environment: "+env.EnvironmentName);
            //    await context.Response.WriteAsync("");
            //});
        }
    }
}
