using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RoutingServer
{
    [ServiceContract]
    public interface INavigationveloserviceSOAP
    {

        [OperationContract]
        string getCheminAVelo(string Depart, string Arrivee);
    }
}