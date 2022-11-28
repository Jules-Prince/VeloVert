using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace RoutingServer
{


    public class NavigationveloserviceSOAP : INavigationveloserviceSOAP
    {
        [return: XmlElement("Chemin", Form = XmlSchemaForm.Unqualified)]
        public Positions getCheminAVelo([XmlElement(Form = XmlSchemaForm.Unqualified)] string Depart, [XmlElement(Form = XmlSchemaForm.Unqualified)] string Arrivee)
        {
            /**
             * 1. Calls OpenStreetMap to retrieve information about the
             * given address.
             */

            OSMProcess osmProcess = new OSMProcess();
            osmProcess.run(Depart, Arrivee);
            osmProcess.printOSMCoordiante();


            /**
             * 2. Calls JCDecaux to retrieve the stations and find: the
             * closest one from the origin with available bikes, and the
             * closest from the destination with places to drop the bike.
             */

            JCDecauxProcess jCDecauxProcess = new JCDecauxProcess();
            jCDecauxProcess.run(osmProcess.OSMCoordinateA, osmProcess.OSMCoordinateB);
            jCDecauxProcess.printJCDevauxCoordinate();

            /*
             * 3.If there are stations matching the conditions, calls
             * OpenStreetMap for 3 itineraries: Origin to Station1,
             * Station1 to Station2, Station2 to Destination
             */

            //https://api.openrouteservice.org/v2/directions/driving-car?api_key=your-api-key&start=8.681495,49.41461&end=8.687872,49.420318
            DirectionProcess directionProcess = new DirectionProcess();
            Positions positions = directionProcess.run(osmProcess.positionA, jCDecauxProcess.positionA, jCDecauxProcess.positionB, osmProcess.positionB);

            return positions;
        }
    }
}
