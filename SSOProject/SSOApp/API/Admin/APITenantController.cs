using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSOApp.ViewModels;
using SSOApp.Controllers.UI;

namespace SSOApp.API.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class APITenantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public APITenantController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("getddtenant")]
        public async Task<List<SelectListItem>> GetDDTenant()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result = await _context.Tenants.Select(d => new SelectListItem { Text = d.Name, Value = d.Code }).ToListAsync();
            result.Insert(0, new SelectListItem { Text = "Select All", Value = "" });
            return result;
        }

        [HttpGet("gettenantbycode")]
        public async Task<Tenant> GetTenantByCode(string code)
        {
            var result = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == code);
            return result;
        }

    }
}