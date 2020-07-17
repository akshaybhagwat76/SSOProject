using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer;
using App.SQLServer.Data;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SSOApp.Models;

namespace SSOApp.API
{
    [Route("[controller]")]
    [ApiController]
    public class AppAccountController : ControllerBase
    {
        private ApplicationDbContext _context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        public AppAccountController(
           UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }
        [HttpGet("gettenant")]
        public async Task<IActionResult> GetTanant(string teancode)
        {
            
            var chcek = await _userManager.Users.FirstOrDefaultAsync(d => d.TenantCode == teancode);
            if(chcek!=null)
                return Ok(new
                {
                    Status = "Tenant Found"
                });

            return Ok(new
            {
                Status = "Tenant Not Found"
            });
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterViewModel model)
        {
            try
            {
                DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
                using (var context = new ApplicationDbContext(options))
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null)
                        return Ok(new
                        {
                            Status = "Duplicate Username"
                        });
                    else
                    {
                        var user1 = new ApplicationUser { UserName = model.Email,
                            Email = model.Email, TenantCode = model.TenantCode,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsActive = true
                        };
                        var createduser=await _userManager.CreateAsync(user1, model.Password);
                        if (createduser.Succeeded)
                        {
                            return Ok(new
                            {
                                Status = "Success"
                            });                            
                        }
                        
                        return Ok(new
                        {
                            Status = "Failed"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = "Failed"
                });
            }           
        }
        [HttpGet("callapi")]
        public async Task<string> CallApi()
        {
            var accessToken1 = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            var array = JArray.Parse(content).ToString();
            return  array ;
        }

        [HttpGet("requesttoken")]
        public async Task<IActionResult> RequestTokenAsync(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
          
            return new JsonResult(token);
        }
       
    }
}