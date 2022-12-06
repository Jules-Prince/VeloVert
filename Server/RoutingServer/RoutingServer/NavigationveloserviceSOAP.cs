using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace RoutingServer
{


    public class NavigationveloserviceSOAP : INavigationveloserviceSOAP
    {
        public string getCheminAVelo(string Depart, string Arrivee)
        {
            Guid guid;

            Console.WriteLine("\n\n=======================================================\n\tNEW REQUEST\n");
            Console.WriteLine($"Depart : {Depart} \nArrivee : {Arrivee}");
            Console.WriteLine("\n[ 1 ] Calls OpenStreetMap to retrieve information about the given address");
            OSMProcess osmProcess = new OSMProcess();
            JCDecauxProcess jCDecauxProcess = new JCDecauxProcess();
            ActiveMQ activeMQ = new ActiveMQ();

            /**
             * 1. Calls OpenStreetMap to retrieve information about the
             * given address.
             */

            Boolean address_found = false;
            address_found = osmProcess.run(Depart, Arrivee);
            if(address_found == false)
            {
                Console.WriteLine(osmProcess.errorMessage);
                guid = activeMQ.errorProducer(osmProcess.errorMessage);
                return guid.ToString();
            }
            osmProcess.printOSMCoordiante();


            /**
             * 2. Calls JCDecaux to retrieve the stations and find: the
             * closest one from the origin with available bikes, and the
             * closest from the destination with places to drop the bike.
             */
            Console.WriteLine("\n [ 2 ] Calls JCDecaux to retrieve the stations and find");
            string cityA = osmProcess.OSMCoordinateA.city;
            double latitudeA = osmProcess.OSMCoordinateA.latitude;
            double longitudeA = osmProcess.OSMCoordinateA.longitude;

            string cityB = osmProcess.OSMCoordinateB.city;
            double latitudeB = osmProcess.OSMCoordinateB.latitude;
            double longitudeB = osmProcess.OSMCoordinateB.longitude;

            Boolean station_found = false;
            station_found = jCDecauxProcess.run(cityA, latitudeA, longitudeA, cityB, latitudeB, longitudeB);
            if (station_found == false)
            {
                Console.WriteLine(jCDecauxProcess.errorMessage);
                guid = activeMQ.errorProducer(jCDecauxProcess.errorMessage);
                return guid.ToString();
            }
            jCDecauxProcess.printJCDevauxCoordinate();

            /*
             * 3.If there are stations matching the conditions, calls
             * OpenStreetMap for 3 itineraries: Origin to Station1,
             * Station1 to Station2, Station2 to Destination
             */
            Console.WriteLine("\n[ 3 ] calls OpenStreetMap for 3 itineraries");
            //https://api.openrouteservice.org/v2/directions/driving-car?api_key=your-api-key&start=8.681495,49.41461&end=8.687872,49.420318
            DirectionProcess directionProcess = new DirectionProcess();
            Positions positions = directionProcess.run(osmProcess.positionA, jCDecauxProcess.positionA, jCDecauxProcess.positionB, osmProcess.positionB);


            /**
             * 4.ActiveMQ
             */

            Console.WriteLine("\n[ 4 ] ActiveMQ");
            guid = activeMQ.producer(positions);

            return guid.ToString();
        }
    }
}
