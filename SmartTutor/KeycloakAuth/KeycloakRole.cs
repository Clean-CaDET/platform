using System;
using Microsoft.AspNetCore.Authorization;

namespace SmartTutor.KeycloakAuth
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