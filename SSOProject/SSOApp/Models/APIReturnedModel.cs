using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.Models
{
    public class APIReturnedModel
    {
        public string status { get; set; }

        public string token { get; set; }
        public string expiration { get; set; }
    }
}
