using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RoutingServer
{
    public class OSMProcess
    {
        /**
         * This class treats the request to the open street map api concerning the retrieval of information of a city. 
         * This information includes latitude, longitude and name of the city. 
         */

        public OSMCoordinate OSMCoordinateA { get; set; }
        public OSMCoordinate OSMCoordinateB { get; set; }

        public string errorMessage { get; set; }

        public Position positionA { get; set; }
        public Position positionB { get; set; }

        public OSMProcess()
        {
            OSMCoordinateA = new OSMCoordinate();
            OSMCoordinateB = new OSMCoordinate();
            positionA = new Position();
            positionB = new Position();
        }

        /**
         * This function runs the process. It returns a boolean. 
         * This boolean allows us to know if the query has succeeded in finding a city. 
         */
        public Boolean run(string adressA, string adressB)
        {
            string urlAPI = "https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf624857ddfd522faa498cb4d1d74518230dff&text=";

            Root rootA = buildDeserializedClass(urlAPI, adressA);
            Root rootB = buildDeserializedClass(urlAPI, adressB);

            // Error management
            if (rootA.features.Count == 0 && rootB.features.Count == 0)
            {
                this.errorMessage = "Unknown addresses : [ " + adressA + " ] and [ " + adressB + " ]";
                return false;
            }else if(rootA.features.Count == 0)
            {
                this.errorMessage = "Unknown addresse : [ " + adressA + " ]";
                return false;
            }else if (rootB.features.Count == 0)
            {
                this.errorMessage = "Unknown addresse : [ " + adressB + " ]";
                return false;
            }

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

            return true;
        }

        /**
         * This method calls the open street map api. 
         * Deserialize the JSON return in an object, and return it. 
         * From this object, we will have all the information we need 
         * to process the information we consider useful.
         */
        private Root buildDeserializedClass(string urlAPI, string param)
        {
            ApiManager aPIManager = new ApiManager();
            string result = aPIManager.APICall(aPIManager.formatUrl(urlAPI), param).Result;
            return JsonConvert.DeserializeObject<Root>(result);
        }

        /**
         * This method allows you to display information about the city. 
         * Longitude latitude and city name.
         */
        public void printOSMCoordiante()
        {
            Console.WriteLine("A longitude : " + OSMCoordinateA.longitude);
            Console.WriteLine("A latitude : " + OSMCoordinateA.latitude);
            Console.WriteLine("A city : " + OSMCoordinateA.city);
            Console.WriteLine("B longitude : " + OSMCoordinateB.longitude);
            Console.WriteLine("B latitude : " + OSMCoordinateB.latitude);
            Console.WriteLine("B city : " + OSMCoordinateB.city);
        }
    }
}
