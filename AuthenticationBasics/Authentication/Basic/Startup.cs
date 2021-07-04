using Basic.AuthorizationRequirements;
using Basic.CustomPolicyProvider;
using Basic.Transformers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basic
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                    .AddCookie("CookieAuth", config =>
                     {
                         config.Cookie.Name = "WebTech.Cookie";
                         config.LoginPath = "/Home/Authenticate";
                     });

            services.AddAuthorization(config =>
            {
                //var defultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defultAuthBuilder
                //.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.DateOfBirth)
                //.Build();

                //config.DefaultPolicy = defaultAuthPolicy;

                //config.AddPolicy("Claim.DOB", policyBuilder =>
                // {
                //     policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                // });

                config.AddPolicy("Claim.DOB", policyBuilder =>
                {
                    //policyBuilder.AddRequirements(new CustomeRequireClaim(ClaimTypes.DateOfBirth));
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, SecurityLevelHandler>();
            services.AddScoped<IAuthorizationHandler, CustomeRequireClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            services.AddControllersWithViews(config=> {
                //var defultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defultAuthBuilder
                //.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.DateOfBirth)
                //.Build();
                //config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();


            //How are you
            app.UseAuthentication();

            //Are you allowed
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
