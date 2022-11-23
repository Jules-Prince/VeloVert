using MyRoutingServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_for_Self_Hosted_WS
{
    internal class OSMProcess
    {
        public OSMCoordinate OSMCoordinateA { get; set; }
        public OSMCoordinate OSMCoordinateB { get; set; }

        public OSMProcess()
        {
            OSMCoordinateA = new OSMCoordinate();
            OSMCoordinateB = new OSMCoordinate();
        }

        public void run(string adressA, string adressB)
        {
            Root rootA = buildDeserializedClass(adressA);
            Root rootB = buildDeserializedClass(adressB);

            // Coordinate A
            OSMCoordinateA.latitude = rootA.features[0].geometry.coordinates[0];
            OSMCoordinateA.longitude = rootA.features[0].geometry.coordinates[1];

            // Coordiante B
            OSMCoordinateB.latitude = rootB.features[0].geometry.coordinates[0];
            OSMCoordinateB.longitude = rootB.features[0].geometry.coordinates[1];
        }

        private Root buildDeserializedClass(string adress)
        {
            OSMModel osm = new OSMModel();
            string result = osm.OSMAPICall(osm.formatUrl(adress)).Result;
            return JsonConvert.DeserializeObject<Root>(result);
        }

        public void printOSMCoordiante()
        {
            Console.WriteLine("A longitude : " + OSMCoordinateA.longitude);
            Console.WriteLine("A latitude : " + OSMCoordinateA.latitude);
            Console.WriteLine("B longitude : " + OSMCoordinateB.longitude);
            Console.WriteLine("B latitude : " + OSMCoordinateB.latitude);
            Console.ReadLine();
        }
    }
}
