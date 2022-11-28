using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RoutingServer
{
    internal class OSMProcess
    {
        public OSMCoordinate OSMCoordinateA { get; set; }
        public OSMCoordinate OSMCoordinateB { get; set; }

        public Position positionA { get; set; }
        public Position positionB { get; set; }

        public OSMProcess()
        {
            OSMCoordinateA = new OSMCoordinate();
            OSMCoordinateB = new OSMCoordinate();
            positionA = new Position();
            positionB = new Position();
        }

        public void run(string adressA, string adressB)
        {
            string urlAPI = "https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf624857ddfd522faa498cb4d1d74518230dff&text=";

            Root rootA = buildDeserializedClass(urlAPI, adressA);
            Root rootB = buildDeserializedClass(urlAPI, adressB);

            // Coordinate A
            OSMCoordinateA.city = rootA.features[0].properties.locality;
            OSMCoordinateA.longitude = rootA.features[0].geometry.coordinates[0];
            OSMCoordinateA.latitude = rootA.features[0].geometry.coordinates[1];
            positionA.longitude = OSMCoordinateA.longitude;
            positionA.latitude = OSMCoordinateA.latitude;

            // Coordiante B
            OSMCoordinateB.city = rootB.features[0].properties.locality;
            OSMCoordinateB.longitude = rootB.features[0].geometry.coordinates[0];
            OSMCoordinateB.latitude = rootB.features[0].geometry.coordinates[1];
            positionB.longitude = OSMCoordinateB.longitude;
            positionB.latitude = OSMCoordinateB.latitude;
        }

        private Root buildDeserializedClass(string urlAPI, string param)
        {
            APIManager aPIManager = new APIManager();
            string result = aPIManager.APICall(aPIManager.formatUrl(urlAPI), param).Result;
            return JsonConvert.DeserializeObject<Root>(result);
        }

        public void printOSMCoordiante()
        {
            Console.WriteLine("A longitude : " + OSMCoordinateA.longitude);
            Console.WriteLine("A latitude : " + OSMCoordinateA.latitude);
            Console.WriteLine("A city : " + OSMCoordinateA.city);
            Console.WriteLine("B longitude : " + OSMCoordinateB.longitude);
            Console.WriteLine("B latitude : " + OSMCoordinateB.latitude);
            Console.WriteLine("B city : " + OSMCoordinateB.city);
            Console.ReadLine();
        }
    }
}
