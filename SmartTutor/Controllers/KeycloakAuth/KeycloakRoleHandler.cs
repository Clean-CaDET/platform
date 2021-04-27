using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SmartTutor.Controllers.KeycloakAuth
{
    public class KeycloakRoleHandler : AuthorizationHandler<KeycloakRole>
    {
        public KeycloakRoleHandler(IWebHostEnvironment env)
        {
            Environment = env;
        }

        private IWebHostEnvironment Environment { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            KeycloakRole requirement)
        {
            if (Environment.IsDevelopment() || context.User.IsInRole(requirement.AllowedRole))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}