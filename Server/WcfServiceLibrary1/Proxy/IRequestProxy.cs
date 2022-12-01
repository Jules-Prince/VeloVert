using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Data.SqlTypes;

namespace Proxy
{
    [ServiceContract]
    internal interface IRequestProxy
    {
        [OperationContract]
        string JCDecauxRequest(string city);
    }
}
