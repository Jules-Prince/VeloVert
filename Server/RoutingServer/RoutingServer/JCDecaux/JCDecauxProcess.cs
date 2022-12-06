using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoutingServer.MyProxy;

namespace RoutingServer
{
    public class JCDecauxProcess
    {

        public Position positionA { set; get; }
        public Position positionB { set; get; }

        public JCDecauxProcess()
        {
            this.positionA = new Position();
            this.positionB = new Position();
        }

        public Boolean run(string cityA, double latitudeA, double longitudeA, string cityB, double latitudeB, double longitudeB)
        {
            string urlAPI = "https://api.jcdecaux.com/vls/v3/stations?apiKey=56c5d019b4d68c1bd60800a1345b299bc7bb95b0&contract=";

            // 1 Récuperer la listes des stations avec le nom de la ville
            List<RootJCDecauxItem> rootA = buildDeserializedClass(urlAPI, cityA);
            //printLatitudeLongitudeStation(rootA);

            List<RootJCDecauxItem> rootB = buildDeserializedClass(urlAPI, cityB);
            //printLatitudeLongitudeStation(rootB);

            if (rootA.Count == 0 || rootB.Count == 0)
            {
                return false;
            }

            // 2 Chercher la station la plus proche avec les coordonnées gps
            RootJCDecauxItem rootJCDecauxDataA = findStationMoreClosed(rootA, longitudeA, latitudeA, Direction.Start);
            RootJCDecauxItem rootJCDecauxDataB = findStationMoreClosed(rootB, longitudeB, latitudeB, Direction.End);

            if(rootJCDecauxDataA == null || rootJCDecauxDataB == null)
            {
                return false;
            }

            // 3 J'enregistre les coordonnées dans la classe adéquate.
            this.positionA.latitude = rootJCDecauxDataA.position.latitude;
            this.positionA.longitude = rootJCDecauxDataA.position.longitude;

            this.positionB.latitude = rootJCDecauxDataB.position.latitude;
            this.positionB.longitude = rootJCDecauxDataB.position.longitude;

            return true;
        }

        private RootJCDecauxItem findStationMoreClosed(List<RootJCDecauxItem> root, double longitude, double latitude, Direction direction)
        {
            RootJCDecauxItem myStation = null;
            double distance = 99999999999;
            double res;

            List<RootJCDecauxItem> newFilteredRootList = deletingInvalidStations(direction, root); 

            foreach (RootJCDecauxItem rootJC in newFilteredRootList)
            {
                if (myStation == null)
                {
                    myStation = rootJC;
                }
                else
                {
                    res = distancePythagore(rootJC, longitude, latitude);
                    if (res < distance)
                    {
                        distance = res;
                        myStation = rootJC;
                    }
                }
            }
            return myStation;
        }

        private List<RootJCDecauxItem> deletingInvalidStations(Direction direction, List<RootJCDecauxItem> root)
        {
            List<RootJCDecauxItem> newList = new List<RootJCDecauxItem>();

            foreach(RootJCDecauxItem rootJC in root)
            {
                if (rootJC.status.Equals("OPEN")){
                    switch (direction)
                    {
                        case Direction.Start:
                            // There is at least one bike left.
                            if (rootJC.totalStands.availabilities.bikes > 0)
                            {
                                newList.Add(rootJC);
                            }
                            break;

                        case Direction.End:
                            // There is at least one place left to park the bike.
                            if (rootJC.totalStands.availabilities.stands > 0)
                            {
                                newList.Add(rootJC);
                            }
                            break;
                    }
                }
            }

            return newList;
        }

        private enum Direction
        {
            Start,
            End
        }

        private double distancePythagore(RootJCDecauxItem rootJC, double longitude, double latitude)
        {
            // Pythagore 
            double x = Math.Abs(longitude - rootJC.position.longitude);
            double y = Math.Abs(latitude - rootJC.position.latitude);
            double d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return d;
        }

        private List<RootJCDecauxItem> buildDeserializedClass(string urlAPI, string param)
        {
            ApiManager aPIManager = new ApiManager();

            //string result = aPIManager.APICall(aPIManager.formatUrl(urlAPI), param).Result;
            try
            {
                MyProxy.RequestProxyClient requestProxy = new MyProxy.RequestProxyClient();
                JCDecauxItem result = requestProxy.JCDecauxRequest(param);
                return this.convert(result.root);
            }
            catch (Exception ex)
            {
                return new List<RootJCDecauxItem>();
            }
        }

        private List<RootJCDecauxItem> convert(RoutingServer.MyProxy.RootJCDecauxItem[] list)
        {
            List<RootJCDecauxItem> myNewList = new List<RootJCDecauxItem>();
            foreach (RootJCDecauxItem elt in list) { 
                myNewList.Add(elt);
            }
            return myNewList;
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
