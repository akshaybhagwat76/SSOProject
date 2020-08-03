using System;
using System.Linq;
using System.Security.Claims;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc;
using SSOApp.Proxy;

namespace SSOApp.Controllers.Home
{

    public class BaseController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly IAPIClientProxy _client;


        public BaseController(ApplicationDbContext context, IAPIClientProxy clientProxy)
        {
            _context = context;
            _client = clientProxy;
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