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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SSOApp.API.ViewModels;

namespace SSOApp.API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private UserManager<ApplicationUser> _userManager;
        public IdentityController(
          IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }        

        [HttpPost("login")]        
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserID);
           
            if (user != null)
            {

                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
              
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

                var token = new JwtSecurityToken(
                    issuer: "http://localhost:5001/",
                    audience: "http://localhost:5001/",
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
               
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpGet("requesttoken")]
        public IActionResult RequestTokenAsync(string userId)
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