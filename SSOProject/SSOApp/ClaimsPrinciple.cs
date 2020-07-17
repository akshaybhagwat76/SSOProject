using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSOApp
{
    public class ClaimsPrincipleExtended:ClaimsPrincipal
    {
        public ClaimsPrincipleExtended(ClaimsPrincipal principal)
        : base(principal)
        {
        }
        public string TenantCode
        {
            get
            {
                return this.FindFirst("TenantCode").Value;
            }
        }

        public string TenantName
        {
            get
            {
                return this.FindFirst("TenantName").Value;
            }
        }
    }
}
