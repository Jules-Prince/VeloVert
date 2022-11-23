using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRoutingServer
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public List<double> bbox { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Root
    {
        public List<Feature> features { get; set; }
    }

}
