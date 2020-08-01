using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SSOApp.Controllers.Home
{
    [ViewComponent(Name = "Module")]
    public class BaseController : Controller
    {
        public readonly ApplicationDbContext _context;


        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ClaimsPrincipleExtended CurrentUser
        {
            get
            {
                return new ClaimsPrincipleExtended(this.User as ClaimsPrincipal);
            }
        }

        public string TenantCode
        {
            get
            {
                return CurrentUser.Claims.FirstOrDefault(c => c.Type == "TenantCode")?.Value;
            }
        }
        public string TenantName
        {
            get
            {
                return CurrentUser.Claims.FirstOrDefault(c => c.Type == "TenantName")?.Value;
            }
        }

        public Guid TenantId
        {
            get
            {
                return new Guid(CurrentUser.Claims.FirstOrDefault(c => c.Type == "TenantID")?.Value);
            }
        }

        public string UserName
        {
            get
            {
                return CurrentUser.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
            }
        }
        public string UserId
        {
            get
            {
                return CurrentUser.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            }
        }

        public string FullName
        {
            get
            {
                return CurrentUser.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
            }
        }

        public string RoleId
        {
            get
            {
                return CurrentUser.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            }
        }

        public Tenant UsersTenant
        {
            get
            {
                return _context.Tenants.Where(m => m.Code == TenantCode).FirstOrDefault();
            }
        }
    }
}