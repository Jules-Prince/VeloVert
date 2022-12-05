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
            // Create the binding
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.MaxBufferSize = 256000000;
            myBinding.MaxReceivedMessageSize = 256000000;


            //Create a URI to serve as the base address
            //Be careful to run Visual Studio as Admistrator or to allow VS to open new port netsh command. 
            // Example : netsh http add urlacl url=http://+:80/MyUri user=DOMAIN\user
            Uri httpUrl = new Uri("http://localhost:8081/NavigationveloserviceSOAP/");

            //Create ServiceHost
            ServiceHost host = new ServiceHost(typeof(NavigationveloserviceSOAP), httpUrl);


            // Multiple end points can be added to the Service using AddServiceEndpoint() method.
            // Host.Open() will run the service, so that it can be used by any client.

            // Example adding :
            // Uri tcpUrl = new Uri("net.tcp://localhost:8090/MyService/SimpleCalculator");
            // ServiceHost host = new ServiceHost(typeof(MyCalculatorService.SimpleCalculator), httpUrl, tcpUrl);

            //Add a service endpoint
            host.AddServiceEndpoint(typeof(INavigationveloserviceSOAP), new BasicHttpBinding(), "Soap");

            

            //ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(INavigationveloserviceSOAP), new WebHttpBinding(), "Web");
            //endpoint.Behaviors.Add(new WebHttpBehavior());

            //Enable metadata exchange
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            //Start the Service
            host.Open();

            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");

            Console.ReadLine();
        }
    }
}
