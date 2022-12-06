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
        /*
         * This method takes as parameters two strings which correspond to the place 
         * of departure and the place of arrival desired. 
         * 
         * This method returns a string that corresponds to a GUID to connect to 
         * activeMQ, or an error message. 
         */

        [OperationContract]
        string getCheminAVelo(string Depart, string Arrivee);
    }
}