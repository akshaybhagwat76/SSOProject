using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SSOApp.Controllers.Home
{
    public class BaseController : Controller
    {
        public ClaimsPrincipleExtended CurrentUser
        {
            get
            {
                return new ClaimsPrincipleExtended(this.User as ClaimsPrincipal);
            }
        }
    }
}