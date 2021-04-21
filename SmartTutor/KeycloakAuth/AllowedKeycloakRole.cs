using System;
using Microsoft.AspNetCore.Authorization;

namespace SmartTutor.KeycloakAuth
{
    public class AllowedKeycloakRole : IAuthorizationRequirement
    {
        public AllowedKeycloakRole(string role)
        {
            AllowedRole = role;
        }

        public string AllowedRole { get; }
    }
}