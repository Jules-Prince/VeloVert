using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RoutingServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Create a URI to serve as the base address
            //Be careful to run Visual Studio as Admistrator or to allow VS to open new port netsh command. 
            // Example : netsh http add urlacl url=http://+:80/MyUri user=DOMAIN\user
            Uri httpUrl = new Uri("http://localhost:8081/navigationveloservice/");
            //Uri httpUrl = new Uri("http://192.168.43.224:8081/navigationveloservice/");

            //Create ServiceHost
            ServiceHost host = new ServiceHost(typeof(NavigationveloserviceSOAP), httpUrl);
            

            // Multiple end points can be added to the Service using AddServiceEndpoint() method.
            // Host.Open() will run the service, so that it can be used by any client.

            // Example adding :
            // Uri tcpUrl = new Uri("net.tcp://localhost:8090/MyService/SimpleCalculator");
            // ServiceHost host = new ServiceHost(typeof(MyCalculatorService.SimpleCalculator), httpUrl, tcpUrl);

            //Add a service endpoint
            host.AddServiceEndpoint(typeof(INavigationveloserviceSOAP), new WSHttpBinding(), "");

            //Enable metadata exchange
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            //Start the Service
            host.Open();

            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");

            test();

            Console.ReadLine();
        }

        static public void test()
        {
            /**
             * 1. Calls OpenStreetMap to retrieve information about the
             * given address.
             */

            string Depart = "33 Rue Edouard Nieuport, 69008 Lyon";
            //string Arrivee = "4 Pl. du Marché, 69009 Lyon";
            //string Depart = "57 avenue de la gare 06800 Cagne sur mer";
            //string Arrivee = "4 Pl. du Marché, 69009 Lyon";
            string Arrivee = "12 Bd Fernand Bonnefoy, 13010 Marseille";
            /**
                * 1. Calls OpenStreetMap to retrieve information about the
                * given address.
                */

            OSMProcess osmProcess = new OSMProcess();
            osmProcess.run(Depart, Arrivee);
            osmProcess.printOSMCoordiante();


            /**
             * 2. Calls JCDecaux to retrieve the stations and find: the
             * closest one from the origin with available bikes, and the
             * closest from the destination with places to drop the bike.
             */
            string cityA = osmProcess.OSMCoordinateA.city;
            double latitudeA = osmProcess.OSMCoordinateA.latitude;
            double longitudeA = osmProcess.OSMCoordinateA.longitude;

            string cityB = osmProcess.OSMCoordinateB.city;
            double latitudeB = osmProcess.OSMCoordinateB.latitude;
            double longitudeB = osmProcess.OSMCoordinateB.longitude;

            JCDecauxProcess jCDecauxProcess = new JCDecauxProcess();
            jCDecauxProcess.run(cityA, latitudeA, longitudeA, cityB, latitudeB, longitudeB);
            jCDecauxProcess.printJCDevauxCoordinate();

            /*
             * 3.If there are stations matching the conditions, calls
             * OpenStreetMap for 3 itineraries: Origin to Station1,
             * Station1 to Station2, Station2 to Destination
             */

            //https://api.openrouteservice.org/v2/directions/driving-car?api_key=your-api-key&start=8.681495,49.41461&end=8.687872,49.420318
            DirectionProcess directionProcess = new DirectionProcess();
            Positions positions = directionProcess.run(osmProcess.positionA, jCDecauxProcess.positionA, jCDecauxProcess.positionB, osmProcess.positionB);


            Guid guid = Guid.NewGuid();
            ActiveMQ activeMQ = new ActiveMQ();
            activeMQ.producer(positions, guid);
            
                        
            Console.WriteLine("TEST OK");
        }
    }
}
