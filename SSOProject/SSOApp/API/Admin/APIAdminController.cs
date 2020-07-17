using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.SQLServer.Data;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SSOApp.API.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class APIAdminController : ControllerBase
    {
        #region Startup
        private ApplicationDbContext _context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IHttpContextAccessor _httpContextAccessor;

        public APIAdminController(
           UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }
        #endregion

        #region APIS
        [HttpGet("createdefaultadmin")]
        public async Task CreateDefaultAdmin()
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);
            }
            else
            {
                ApplicationUser findUser = await _userManager.FindByNameAsync("admin");
                if (findUser == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = "admin";
                    user.Email = "admin@test.com";
                    user.TenantCode = "ABCO";
                    string userPWD = "Admin123!";

                    IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Succeeded)
                    {
                        var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }
        [HttpGet("createrolesandadmin")]
        public async Task CreateRolesandUsers()
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@test.com";
                user.TenantCode = "ABCO";
                string userPWD = "Admin123!";

                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("Manager");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Employee role     
            x = await _roleManager.RoleExistsAsync("Team");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);
            }
        }
        #endregion
    }
}