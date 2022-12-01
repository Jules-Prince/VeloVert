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
        private GenericProxyCache<JCDecauxItem> proxy;

        public RequestProxy() {
            this.proxy = new GenericProxyCache<JCDecauxItem>();
        }

        public JCDecauxItem JCDecauxRequest(string city)
        {
            Console.WriteLine("");
            Console.WriteLine("Resquet for city : " + city);

            return this.proxy.Get(city);
            //return this.proxy.Get(city, 60);
            //return this.proxy.Get(city, DateTimeOffset.Now.AddSeconds(60));
        }
    }
}
