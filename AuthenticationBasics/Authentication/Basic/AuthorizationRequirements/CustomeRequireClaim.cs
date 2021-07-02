using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Basic.AuthorizationRequirements
{
    public class CustomeRequireClaim : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public CustomeRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }
    }

    public class CustomeRequireClaimHandler : AuthorizationHandler<CustomeRequireClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomeRequireClaim requirement)
        {
            if(context.User.Claims.Any(x => x.Type == requirement.ClaimType))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtentions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder,string claim)
        {
            builder.AddRequirements(new CustomeRequireClaim(claim));
            return builder;
        }
    }
}
