using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.JwtRequrements
{
    public class JwtRequrement:IAuthorizationRequirement
    {}

    public class JwtRequrementHandler : AuthorizationHandler<JwtRequrement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;

        public JwtRequrementHandler(IHttpClientFactory httpClientFactory,IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequrement requirement)
        {
            if(_httpContext.Request.Headers.TryGetValue("Authorization",out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];
                var response = await _client.GetAsync($"https://localhost:44312/oauth/validate?access_token={accessToken}");

                if(response.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
