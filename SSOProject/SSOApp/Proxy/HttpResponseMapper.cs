using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SSOApp.Proxy
{
    public class Response<T>
    {
        public HttpStatusCode Status { get; set; }

        public string Message { get; set; }
        
        public string ActionResponseCode { get; set; }

        public T Body { get; set; }

        public string PageTitle { get; set; }

        public string PageSubheading { get; set; }
    }
}
