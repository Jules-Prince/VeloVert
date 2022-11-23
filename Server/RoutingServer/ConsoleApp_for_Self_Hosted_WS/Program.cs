using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add the WCF ServiceModel namespace 
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net.Http;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using ConsoleApp_for_Self_Hosted_WS;

namespace MyRoutingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a URI to serve as the base address
            //Be careful to run Visual Studio as Admistrator or to allow VS to open new port netsh command. 
            // Example : netsh http add urlacl url=http://+:80/MyUri user=DOMAIN\user
            Uri httpUrl = new Uri("http://localhost:8090/MyService/SimpleCalculator");

            //Create ServiceHost
            ServiceHost host = new ServiceHost(typeof(SimpleCalculator), httpUrl);

            // Multiple end points can be added to the Service using AddServiceEndpoint() method.
            // Host.Open() will run the service, so that it can be used by any client.

            // Example adding :
            // Uri tcpUrl = new Uri("net.tcp://localhost:8090/MyService/SimpleCalculator");
            // ServiceHost host = new ServiceHost(typeof(MyCalculatorService.SimpleCalculator), httpUrl, tcpUrl);

            //Add a service endpoint
            host.AddServiceEndpoint(typeof(ISimpleCalculator), new WSHttpBinding(), ""); 

            //Enable metadata exchange
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);
            
            //Start the Service
            host.Open();

            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");


            // *************************************
            //              REST 
            // *************************************

            string adressA = "33 Rue Edouard Nieuport, 69008 Lyon";
            string adressB = "4 Pl. du Marché, 69009 Lyon";

            ProcessOfObtainingTheSteps process = new ProcessOfObtainingTheSteps();
            process.run(adressA, adressB);

            Console.ReadLine();
        }
    }
}