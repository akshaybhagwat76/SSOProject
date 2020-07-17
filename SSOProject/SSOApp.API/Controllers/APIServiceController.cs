using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SSOApp.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class APIServiceController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public APIServiceController(
          IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("getdata")]
        public ActionResult Get()
        {
            var claims = HttpContext.User.Claims.Select(x => $"{x.Type}:{x.Value}");

            return Ok(new
            {
                Name = "Values API",
                Claims = claims.ToArray()
            });
        }
        [HttpGet("getuserbyid")]
        public async Task<ApplicationUser> GetUserByID(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        [HttpGet("userisinrole")]
        public async Task<bool> UserIsInRole(string username, string rolename)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _userManager.IsInRoleAsync(user, rolename);
        }
        [HttpGet("gettenantbyuser")]
        public async Task<Tenant> GetTenantByUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Tenants.FirstOrDefaultAsync(d => d.Code == user.TenantCode);
        }

        public IActionResult Post()
        {


            return Ok(new
            {
                Status = "Failed"
            });
        }
    }
}