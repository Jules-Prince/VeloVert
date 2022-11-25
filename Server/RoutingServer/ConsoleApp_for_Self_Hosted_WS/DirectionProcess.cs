using MyRoutingServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_for_Self_Hosted_WS
{
    internal class DirectionProcess
    {
        public void run(Position positionA, Position positionB, Position positionC, Position positionD)
        {
            RootDirection rootDirectionAB = findTheWay(positionA, positionB);
            RootDirection rootDirectionBC = findTheWay(positionB, positionC);
            RootDirection rootDirectionCD = findTheWay(positionC, positionD);
            
            foreach(Feature f in rootDirectionAB.features)
            {
                foreach(int i in f.geometry.coordinates)
                {
                    Console.WriteLine(i);
                }
            }
        }

        public RootDirection findTheWay(Position positionA, Position positionB)
        {
            APIManager aPIManager = new APIManager();

            string urlAPI = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=5b3ce3597851110001cf624857ddfd522faa498cb4d1d74518230dff";
            string start = "&start=" + aPIManager.correctPostionFormat(positionA.longitude) + "," + aPIManager.correctPostionFormat(positionA.latitude);
            string end = "&end=" + aPIManager.correctPostionFormat(positionB.longitude) + "," + aPIManager.correctPostionFormat(positionB.latitude);
            string param = start + end;

            Console.WriteLine(urlAPI + param);
            Console.ReadLine();

            string result = aPIManager.APICall(urlAPI, param).Result;
            return JsonConvert.DeserializeObject<RootDirection>(result);
        }
    }
}
