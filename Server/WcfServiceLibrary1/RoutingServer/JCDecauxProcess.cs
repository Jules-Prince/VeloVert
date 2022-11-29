using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RoutingServer
{
    internal class JCDecauxProcess
    {

        public Position positionA { set; get; }
        public Position positionB { set; get; }

        public JCDecauxProcess()
        {
            this.positionA = new Position();
            this.positionB = new Position();
        }

        public void run(OSMCoordinate oA, OSMCoordinate oB)
        {
            string urlAPI = "https://api.jcdecaux.com/vls/v3/stations?apiKey=56c5d019b4d68c1bd60800a1345b299bc7bb95b0&contract=";

            // 1 Récuperer la listes des stations avec le nom de la ville
            List<RootJCDecauxDataAPI> rootA = buildDeserializedClass(urlAPI, oA.city);
            //printLatitudeLongitudeStation(rootA);

            List<RootJCDecauxDataAPI> rootB = buildDeserializedClass(urlAPI, oB.city);
            //printLatitudeLongitudeStation(rootB);

            // 2 Chercher la station la plus proche avec les coordonnées gps
            RootJCDecauxDataAPI rootJCDecauxDataA = findStationMoreClosed(rootA, oA);
            RootJCDecauxDataAPI rootJCDecauxDataB = findStationMoreClosed(rootB, oB);


            // 3 J'enregistre les coordonnées dans la classe adéquate.
            this.positionA.latitude = rootJCDecauxDataA.position.latitude;
            this.positionA.longitude = rootJCDecauxDataA.position.longitude;

            this.positionB.latitude = rootJCDecauxDataB.position.latitude;
            this.positionB.longitude = rootJCDecauxDataB.position.longitude;
        }

        private RootJCDecauxDataAPI findStationMoreClosed(List<RootJCDecauxDataAPI> root, OSMCoordinate osmC)
        {
            RootJCDecauxDataAPI myStation = null;
            double distance = 99999999999;
            double res;

            foreach (RootJCDecauxDataAPI rootJC in root)
            {
                if (myStation == null)
                {
                    myStation = rootJC;
                }
                else
                {
                    res = distancePythagore(rootJC, osmC);
                    if (res < distance)
                    {
                        distance = res;
                        myStation = rootJC;
                    }
                }
            }
            return myStation;
        }

        private double distancePythagore(RootJCDecauxDataAPI rootJC, OSMCoordinate osmC)
        {
            // Pythagore 
            double x = Math.Abs(osmC.longitude - rootJC.position.longitude);
            double y = Math.Abs(osmC.latitude - rootJC.position.latitude);
            double d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return d;
        }

        private List<RootJCDecauxDataAPI> buildDeserializedClass(string urlAPI, string param)
        {
            APIManager aPIManager = new APIManager();
            //string result = aPIManager.APICall(aPIManager.formatUrl(urlAPI), param).Result;
            RequestProxy.RequestProxyClient request = new RequestProxy.RequestProxyClient();
            string result = request.JCDecauxRequest(param);
            return JsonConvert.DeserializeObject<List<RootJCDecauxDataAPI>>(result);
        }

        /**
         *  For DEBUG
         */
        private void printLatitudeLongitudeStation(List<RootJCDecauxDataAPI> rootA)
        {
            foreach (RootJCDecauxDataAPI r in rootA)
            {
                Console.WriteLine("Latitude : " + r.position.latitude);
                Console.WriteLine("Longitude : " + r.position.longitude);
                Console.WriteLine("");
            }
            Console.ReadLine();
        }

        public void printJCDevauxCoordinate()
        {
            Console.WriteLine("JCDevaux Coordinate A");
            Console.WriteLine("Longitude : " + this.positionA.longitude);
            Console.WriteLine("Latitude : " + this.positionA.latitude);

            Console.WriteLine("JCDevaux Coordinate B");
            Console.WriteLine("Longitude : " + this.positionB.longitude);
            Console.WriteLine("Latitude : " + this.positionB.latitude);
        }
    }
}
