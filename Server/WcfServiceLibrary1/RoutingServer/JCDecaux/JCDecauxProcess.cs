﻿using System;
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

        public void run(string cityA, double latitudeA, double longitudeA, string cityB, double latitudeB, double longitudeB)
        {
            string urlAPI = "https://api.jcdecaux.com/vls/v3/stations?apiKey=56c5d019b4d68c1bd60800a1345b299bc7bb95b0&contract=";

            // 1 Récuperer la listes des stations avec le nom de la ville
            List<RootJCDecauxItem> rootA = buildDeserializedClass(urlAPI, cityA);
            //printLatitudeLongitudeStation(rootA);

            List<RootJCDecauxItem> rootB = buildDeserializedClass(urlAPI, cityB);
            //printLatitudeLongitudeStation(rootB);

            // 2 Chercher la station la plus proche avec les coordonnées gps
            RootJCDecauxItem rootJCDecauxDataA = findStationMoreClosed(rootA, longitudeA, latitudeA);
            RootJCDecauxItem rootJCDecauxDataB = findStationMoreClosed(rootB, longitudeB, latitudeB);


            // 3 J'enregistre les coordonnées dans la classe adéquate.
            this.positionA.latitude = rootJCDecauxDataA.position.latitude;
            this.positionA.longitude = rootJCDecauxDataA.position.longitude;

            this.positionB.latitude = rootJCDecauxDataB.position.latitude;
            this.positionB.longitude = rootJCDecauxDataB.position.longitude;
        }

        private RootJCDecauxItem findStationMoreClosed(List<RootJCDecauxItem> root, double longitude, double latitude)
        {
            RootJCDecauxItem myStation = null;
            double distance = 99999999999;
            double res;

            foreach (RootJCDecauxItem rootJC in root)
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
            MyProxy.RequestProxyClient requestProxy = new MyProxy.RequestProxyClient();
            JCDecauxItem result = requestProxy.JCDecauxRequest(param);
            return this.convert(result.root);
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