using Microsoft.AspNetCore.Authorization;

namespace SmartTutor.Controllers.KeycloakAuth
{
    public class KeycloakRole : IAuthorizationRequirement
    {
        public KeycloakRole(string role)
        {
            AllowedRole = role;
        }

        public string AllowedRole { get; }
    }
}