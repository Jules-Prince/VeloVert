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
        /**
         * This class is responsible for the communication with the proxy, 
         * in order to retrieve the stations of a city. 
         */
        public Position positionA { set; get; } // Position of the closest station to A
        public Position positionB { set; get; } // Position of the closest station to B

        public JCDecauxProcess()
        {
            this.positionA = new Position();
            this.positionB = new Position();
        }

        /**
         * Allows you to run the process of finding the closest location to point A and point B. 
         * This method returns a boolean. If there is no station in the city, then it returns false. Otherwise it returns true. 
         */
        public Boolean run(string cityA, double latitudeA, double longitudeA, string cityB, double latitudeB, double longitudeB)
        {
            string urlAPI = "https://api.jcdecaux.com/vls/v3/stations?apiKey=56c5d019b4d68c1bd60800a1345b299bc7bb95b0&contract=";

            // 1 Retrieve the list of stations with the name of the city
            List<RootJCDecauxItem> rootA = buildDeserializedClass(urlAPI, cityA);
            List<RootJCDecauxItem> rootB = buildDeserializedClass(urlAPI, cityB);

            if (rootA.Count == 0 || rootB.Count == 0)
            {
                return false;
            }

            // 2 Search for the nearest station with gps coordinates
            RootJCDecauxItem rootJCDecauxDataA = findStationMoreClosed(rootA, longitudeA, latitudeA, Direction.Start);
            RootJCDecauxItem rootJCDecauxDataB = findStationMoreClosed(rootB, longitudeB, latitudeB, Direction.End);

            if(rootJCDecauxDataA == null || rootJCDecauxDataB == null)
            {
                return false;
            }

            // 3 I save the coordinates in the variables of the class. 
            this.positionA.latitude = rootJCDecauxDataA.position.latitude;
            this.positionA.longitude = rootJCDecauxDataA.position.longitude;

            this.positionB.latitude = rootJCDecauxDataB.position.latitude;
            this.positionB.longitude = rootJCDecauxDataB.position.longitude;

            return true;
        }

        /**
         * This method returns the Item (the return object of the JCDecaux api) corresponding 
         * to the closest station to the given position. 
         * 
         * The direction is used to know if it is to come to take a bike
         * (So it is to know if there is a bike in the station)
         * , or to come to drop one 
         * (So it is to know if there is space left in the sta
         */
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

        /**
         * This method filters the list of stations in a city, depending on the direction. 
         * If it's to pick up a bike, there must be at least one left. 
         * If it's to drop off a bike, there must be at least one space available. 
         */
        private List<RootJCDecauxItem> deletingInvalidStations(Direction direction, List<RootJCDecauxItem> root)
        {
            List<RootJCDecauxItem> newList = new List<RootJCDecauxItem>();

            foreach(RootJCDecauxItem rootJC in root)
            {
                if (rootJC.status.Equals("OPEN"))  // If the station is open
                {
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

        /**
         * To compare distances, and to find the closest station, we use the Pythagorian theorem. 
         */
        private double distancePythagore(RootJCDecauxItem rootJC, double longitude, double latitude)
        {
            // Pythagore : a² = b² + c²
            double x = Math.Abs(longitude - rootJC.position.longitude);
            double y = Math.Abs(latitude - rootJC.position.latitude);
            double d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return d;
        }

        /**
         * This method asks the proxy to make a query, to get the stations of a city, 
         * then returns the object containing the information. 
         * ( In the proxy, the return json is deserialized and stored as an object ).
         * If there is no response from the proxy, returns an object without content.  
         */
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

        /**
         * This method converts the return object of the proxy into a usable object in this class. 
         * It is just an adapter
         */
        private List<RootJCDecauxItem> convert(RoutingServer.MyProxy.RootJCDecauxItem[] list)
        {
            List<RootJCDecauxItem> myNewList = new List<RootJCDecauxItem>();
            foreach (RootJCDecauxItem elt in list) { 
                myNewList.Add(elt);
            }
            return myNewList;
        }

        /**
         * This method displays the coordinates of the 2 stations. 
         */
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
