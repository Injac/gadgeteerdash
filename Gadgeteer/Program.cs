using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT.Net.NetworkInformation;
using System.Net;


namespace GadgeteerSender
{

   

    public partial class Program
    {
        static bool networkAvailable = false;
       
    
        private static Random r;
        private static HttpRequest req;
        private static HttpRequest req2;
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            /*******************************************************************************************
            Modules added in the Program.gadgeteer designer view are used by typing 
            their name followed by a period, e.g.  button.  or  camera.
            
            Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
                button.ButtonPressed +=<tab><tab>
            
            If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
                GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
                timer.Tick +=<tab><tab>
                timer.Start();
            *******************************************************************************************/


            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
           
            Debug.Print("Program Started");

            Debug.Print("Initializing Network");

            InitEthernet();

            Debug.GC(true);

            while(!networkAvailable)
            {
                Thread.Sleep(250);
            }

            r = new Random();

            SendToWebApi(0);
          
            GT.Timer timer = new GT.Timer(1500);

            timer.Tick += timer_Tick;


            GT.Timer timer2 = new GT.Timer(1000);

            timer2.Tick += timer2_Tick;
          
            timer.Start();
            timer2.Start();
        }

        void timer2_Tick(GT.Timer timer)
        {
                SendToWebApi2(DateTime.Now.ToString("HH:mm:ss"));


        }

        void timer_Tick(GT.Timer timer)
        {
            var rand = r.Next(100);
            SendToWebApi(rand);

        }

        private void SendToWebApi(int percentage)
        {

            req = WebClient.GetFromWeb("http://yoursite.azurewebsites.net/api/percentage?id=" + percentage);
              req.ResponseReceived += client_ResponseReceived;
           
        }

        private void SendToWebApi2(string sometext)
        {

            req2 = WebClient.GetFromWeb("http://yoursite.azurewebsites.net/api/sysinfo?value=" + HTTPUtility.UrlEncode(sometext));
            req2.ResponseReceived += client_ResponseReceived2;

        }

        void client_ResponseReceived(HttpRequest sender, HttpResponse response)
        {
                //Nothing to do here
        }

        void client_ResponseReceived2(HttpRequest sender, HttpResponse response)
        {
                //Nothing to do here
        }

        void hr_ResponseReceived(HttpRequest sender, HttpResponse response)
        {
            throw new NotImplementedException();
        }

   
        private void InitEthernet()
        {

            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            ethernetJ11D.NetworkUp += ethernetJ11D_NetworkUp;
            ethernetJ11D.UseThisNetworkInterface();
            //STATIC IP
            ethernetJ11D.NetworkInterface.EnableStaticIP("[YOURSTATICIP]", "[YOURSUBNET]", "[YOURGATEWAY]");
            ethernetJ11D.NetworkSettings.EnableStaticDns(new string[] { "[YOURDNSSERVER]" });
            //OR DHCP
            //ethernetJ11D.UseDHCP();
            //ethernetJ11D.NetworkInterface.EnableDynamicDns();

        }

        void ethernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network up!");
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("Network Address changed!");
            Debug.Print("Current Network address:");
            Debug.Print(ethernetJ11D.NetworkInterface.IPAddress);
        }

        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            networkAvailable = true;
        }
    }

    public static class HTTPUtility
    {
        #region URL Encoding

        public static string UrlEncode(string s)
        {
            if (s == null)
                return null;

            string result = string.Empty;

            for (int i = 0; i < s.Length; i++)
            {
                if (ShouldEncodeChar(s[i]))
                    result += '%' + ByteToHex((byte)s[i]);
                else
                    result += s[i];
            }

            return result;
        }

        private static bool ShouldEncodeChar(char c)
        {
            // Safe characters defined by RFC3986:
            // http://oauth.net/core/1.0/#encoding_parameters

            if (c >= '0' && c <= '9')
                return false;
            if (c >= 'A' && c <= 'Z')
                return false;
            if (c >= 'a' && c <= 'z')
                return false;
            switch (c)
            {
                case '-':
                case '.':
                case '_':
                case '~':
                    return false;
            }

            // All other characters should be encoded
            return true;
        }

        public static string ByteToHex(byte b)
        {
            const string hex = "0123456789ABCDEF";
            int lowNibble = b & 0x0F;
            int highNibble = (b & 0xF0) >> 4;
            string s = new string(new char[] { hex[highNibble], hex[lowNibble] });
            return s;
        }

        #endregion

        
    }
}
