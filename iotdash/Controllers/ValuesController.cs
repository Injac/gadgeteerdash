using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;

namespace iotdash.Controllers
{
    [AllowAnonymous]
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("api/percentage")]
        // GET api/values/5
        public string Percentage(int id)
        {

            var context = GlobalHost.ConnectionManager.GetHubContext<GadgeteerHub>();
            context.Clients.All.hello(Convert.ToString(id));
            
            return default(string);
        }

        [HttpGet]
        [Route("api/sysinfo")]
        public string SystemInfo(string value)
        {

            var context = GlobalHost.ConnectionManager.GetHubContext<GadgeteerHub>();
            context.Clients.All.systemInfo(value);

            return default(string);
        }
    }
}
