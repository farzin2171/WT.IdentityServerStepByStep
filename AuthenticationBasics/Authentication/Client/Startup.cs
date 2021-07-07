using Client.InfraStructure;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Client
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

            services.AddAuthentication(config =>
            {
                //we check the cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = "ClinetCookie";
                //When we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClinetCookie";
                //Use this to check if we are allowed to do something.
                config.DefaultChallengeScheme = "OurServer";



            })
                .AddCookie("ClinetCookie")
                //https://datatracker.ietf.org/doc/html/rfc6749
                .AddOAuth("OurServer", config =>
                {
                    config.CallbackPath = "/oauth/callback";
                    config.ClientId = "clinet_id";
                    config.ClientSecret = "client_secret";
                    config.AuthorizationEndpoint = "https://localhost:44312/oauth/authorize";
                    config.TokenEndpoint = "https://localhost:44312/oauth/token";

                    config.SaveTokens = true;

                    config.Events = new OAuthEvents
                    {
                        OnCreatingTicket = context =>
                         {
                             var access_token = context.AccessToken;
                             var based64payload = access_token.Split('.')[1];
                             var bytes = Extentions.ConvertFromBase64String(based64payload);
                             var jsonPayload = Encoding.UTF8.GetString(bytes);

                             var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

                             foreach (var claim in claims)
                             {
                                 context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                             }

                             return Task.CompletedTask;
                         }
                    };
                });

            services.AddHttpClient();
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
