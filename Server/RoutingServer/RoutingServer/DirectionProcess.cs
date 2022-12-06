using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RoutingServer
{
    internal class DirectionProcess
    {
        /**
         * This class allows to draw the initinary between all points. 
         * From the starting point to the nearest station. 
         * From the nearest station to the arrival station. 
         * From the arrival station to the arrival place. 
         */

        /**
         * This method runs the process of building the itinerary. 
         */
        public Positions run(Position positionA, Position positionB, Position positionC, Position positionD)
        {
            RootDirection rootDirectionAB = findTheWay(positionA, positionB);
            RootDirection rootDirectionBC = findTheWay(positionB, positionC);
            RootDirection rootDirectionCD = findTheWay(positionC, positionD);
    
            Positions positions= new Positions();
            List<Position> myStep = new List<Position>();

            myStep = addPosition(myStep, rootDirectionAB.features[0].geometry.coordinates, TransportType.Walk);
            myStep = addPosition(myStep, rootDirectionBC.features[0].geometry.coordinates, TransportType.Bike);
            myStep = addPosition(myStep, rootDirectionCD.features[0].geometry.coordinates, TransportType.Walk);

            positions.step = myStep;

            return positions;
        }

        /**
         * From the return object, I retrieve useful information, 
         * such as the latitude and length of all stages of the route. 
         * 
         * This method also marks the type of transportation, 
         * if it is by bike or by foot. This allows to differentiate 
         * the type of transport in the stages. 
         */
        private List<Position> addPosition(List<Position> positions, List<List<double>> coordinateList, TransportType transportType)
        {
            List<Position> p = positions;

            foreach (List<double> coordinates in coordinateList)
            {
                Position position = new Position();
                position.latitude = coordinates[0];
                position.longitude = coordinates[1];

                if(transportType == TransportType.Bike)
                {
                    position.type = "bike";
                }else if(transportType == TransportType.Walk) {
                    position.type = "walk";
                }

                p.Add(position);
            }

            return p;
        }

        private enum TransportType
        {
            Bike = 0,
            Walk = 1
        }

        /**
         * This method returns a RootDirection. 
         * This object contains all the steps of the itinerary between point A and point b.
         */
        public RootDirection findTheWay(Position positionA, Position positionB)
        {
            ApiManager aPIManager = new ApiManager();

            string urlAPI = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=5b3ce3597851110001cf624857ddfd522faa498cb4d1d74518230dff";
            string start = "&start=" + aPIManager.correctPostionFormat(positionA.longitude) + "," + aPIManager.correctPostionFormat(positionA.latitude);
            string end = "&end=" + aPIManager.correctPostionFormat(positionB.longitude) + "," + aPIManager.correctPostionFormat(positionB.latitude);
            string param = start + end;

            Console.WriteLine(urlAPI + param);

            string result = aPIManager.APICall(urlAPI, param).Result;
            return JsonConvert.DeserializeObject<RootDirection>(result);
        }
    }
}
