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

            // Exemple : https://nominatim.openstreetmap.org/search/Unter%20den%20Linden%201%20Berlin?format=json&addressdetails=1&limit=1&polygon_svg=1
            //string url = "https://nominatim.openstreetmap.org/search/";
            //https://nominatim.openstreetmap.org/search?q=%2257%20Avenue%20de%20la%20gare%2006800%20Cagnes%20sur%20mer%22

            string url = "https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf624857ddfd522faa498cb4d1d74518230dff&text=";
            
            string adress = "57 avenue de la gare Cagnes sur mer";
            string result1 = OSMAPICall(url + formatUrl(adress)).Result;

            adress = "930 Rte des Colles, 06410 Biot";
            string result2 = OSMAPICall(url + formatUrl(adress)).Result;

            Console.WriteLine("My result" + result1);
            Console.ReadLine();

            //Test feature = JsonSerializer.Deserialize<Test>(result1);
            //Console.WriteLine("my feature : " + feature.ToString());

            Console.WriteLine("My result" + result2);
            Console.ReadLine();


            
        }

        static public string formatUrl(string adress)
        {
            string url = "";
            foreach(char c in adress) {
                if (c == ' ')
                {
                    url = url + "%";
                    url = url + "2";
                    url = url + "0";
                }
                else
                {
                    url = url + c;
                }
            }
            Console.WriteLine(url);
            Console.ReadLine();
            return url;
        }

        static async Task<string> OSMAPICall(string url)
        {
            Console.WriteLine(url);
            Console.ReadLine() ;

            // DOC : https://nominatim.org/release-docs/latest/api/Search/
            // DOC : https://www.smalsresearch.be/geocodage-contourner-les-lacunes-dopenstreetmap-partie-1/
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }

    public class MyClass
    {
        string features { get; set; }
    }

    public class Test
    {
        Geocoding geocoding { get; set; }
    }

    public class Geocoding
    {
        string version { get; set; }
    }
}