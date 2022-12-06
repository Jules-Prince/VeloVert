using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingServer
{
    public class OSMCoordinate
    {
        /**
         * This class is a POJO that contains all the useful information about the search 
         * of a city with its information via open street map. 
         */
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string city { get; set; }
    }
}
