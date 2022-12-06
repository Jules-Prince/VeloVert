using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingServer
{
    public class JCDevauxCoordinate
    {
        /**
         * This class is a POJO that contains all the useful information 
         * about the search of the stations of a city with its information via the JCDecaux api. 
         */
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
