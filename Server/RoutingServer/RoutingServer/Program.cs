using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web.Services.Description;

namespace RoutingServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ===================================================
            //                     CONFIG
            // ===================================================

            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.MaxBufferSize = 256000000;
            myBinding.MaxReceivedMessageSize = 256000000;

            Uri httpUrl = new Uri("http://localhost:8081/NavigationveloserviceSOAP/");

            ServiceHost host = new ServiceHost(typeof(NavigationveloserviceSOAP), httpUrl);
            host.AddServiceEndpoint(typeof(INavigationveloserviceSOAP), new BasicHttpBinding(), "Soap");

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            //Start the Service
            host.Open();

            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");

            // ===================================================
            //                WE LET THE SERVER OVER
            // ===================================================

            Console.ReadLine();
        }
    }
}
