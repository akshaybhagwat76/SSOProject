using IdentityModel;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSOApp.Models
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser { SubjectId = "1", Username = "admin", Password = "Admin123!",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "admin"),
                    new Claim(JwtClaimTypes.GivenName, "admin"),
                    new Claim(JwtClaimTypes.FamilyName, "admin"),
                    new Claim(JwtClaimTypes.Email, "admin@test.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            }
        };
    }
}
