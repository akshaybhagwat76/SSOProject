using App.SQLServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSOApp
{
    public class ExtendedUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public readonly ApplicationDbContext _context;
        public string TenantName = null;
        public ExtendedUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor, ApplicationDbContext context)
            : base(userManager, optionsAccessor)
        {
            _context = context;

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var Role = UserManager.GetRolesAsync(user);
            var roleId = _context.Roles.FirstOrDefault(x => x.Name == Role.Result[0]);
            if (string.IsNullOrEmpty(TenantName) && Role.Result.Count > 0)
            {
                var tenant = _context.Tenants.Where(m => m.Code == user.TenantCode).FirstOrDefault();
                identity.AddClaim(new Claim("TenantCode", tenant.Code));
                identity.AddClaim(new Claim("TenantName", tenant.Name));
                identity.AddClaim(new Claim("role", Role.Result[0]));
                identity.AddClaim(new Claim("FullName", $"{ user.FirstName } {user.LastName}"));
                identity.AddClaim(new Claim("TenantID", tenant.Id.ToString()));
                identity.AddClaim(new Claim("RoleId", roleId.Id));
            }
            return identity;
        }
    }
}
