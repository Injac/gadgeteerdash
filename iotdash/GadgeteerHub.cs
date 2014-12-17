using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace iotdash
{
    public class GadgeteerHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SystemInfo()
        {
            Clients.All.systemInfo();
        }
    }
}