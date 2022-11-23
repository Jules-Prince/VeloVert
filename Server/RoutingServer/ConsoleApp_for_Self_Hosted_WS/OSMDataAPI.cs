using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRoutingServer
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // https://json2csharp.com/
    public class Root
    {
        public List<Feature> features { get; set; }
    }

    public class Feature
    {
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry
    {
        public List<double> coordinates { get; set; }
    }


    public class Properties
    {
        public string locality { get; set; }
    }
}
