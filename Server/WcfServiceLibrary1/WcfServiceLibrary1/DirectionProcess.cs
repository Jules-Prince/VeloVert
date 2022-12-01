using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WcfServiceLibrary1
{
    internal class DirectionProcess
    {
        public Positions run(Position positionA, Position positionB, Position positionC, Position positionD)
        {
            RootDirection rootDirectionAB = findTheWay(positionA, positionB);
            RootDirection rootDirectionBC = findTheWay(positionB, positionC);
            RootDirection rootDirectionCD = findTheWay(positionC, positionD);
    
            Positions positions= new Positions();
            List<Position> myStep = new List<Position>();

            myStep = addPosition(myStep, rootDirectionAB.features[0].geometry.coordinates);
            myStep = addPosition(myStep, rootDirectionBC.features[0].geometry.coordinates);
            myStep = addPosition(myStep, rootDirectionCD.features[0].geometry.coordinates);

            positions.step = myStep;

            return positions;
        }

        private List<Position> addPosition(List<Position> positions, List<List<double>> coordinateList)
        {
            List<Position> p = positions;

            foreach (List<double> coordinates in coordinateList)
            {
                Position position = new Position();
                position.latitude = coordinates[0];
                position.longitude = coordinates[1];

                p.Add(position);
            }

            return p;
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
