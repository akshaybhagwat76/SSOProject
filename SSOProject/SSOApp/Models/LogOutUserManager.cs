using App.SQLServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.Models
{
    public class LogOutUserManager
    {
        List<SUser> _users = new List<SUser>();
        public void Add(string sub, string sid)
        {
            _users.Add(new SUser { Sub = sub, Sid = sid });
        }
        public bool IsLoggedOut(string sub, string sid)
        {
            var matches = _users.Any(s => s.isMatch(sub, sid));
            return matches;
        }
        private class SUser
        {
            public string Sub { get; set; }
            public string Sid { get; set; }
            public bool isMatch(string sub, string sid)
            {
                return (Sid == sid && Sub == sub) ||
                    (Sid == sid && Sub == null) ||
                    (Sid == null && Sub == sub);
            }
        }
    }
}
