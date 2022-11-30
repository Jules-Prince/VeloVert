using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    internal class RequestProxy : IRequestProxy
    {
        public JCDecauxItem JCDecauxRequest(string city)
        {
            Console.WriteLine("Resquet for city : " + city);
            GenericProxyCache<JCDecauxItem> proxy = new GenericProxyCache<JCDecauxItem>();
            return proxy.Get(city);
        }
    }
}
