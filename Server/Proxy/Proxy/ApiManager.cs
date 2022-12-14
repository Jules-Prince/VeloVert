using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    internal class ApiManager
    {
        public JCDecauxItem run(string city)
        {
            string val = this.request(city);
            return this.GetJCDecauxItem(val);
        } 

        private string request(string city)
        {
            string urlAPI = "https://api.jcdecaux.com/vls/v3/stations?apiKey=56c5d019b4d68c1bd60800a1345b299bc7bb95b0&contract=";
            string url = urlAPI + city;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            string myJson = response.Content.ReadAsStringAsync().Result;

            //Console.WriteLine("URL : " + urlAPI);
            //Console.Write("myJson : " + myJson);

            return myJson;
        }

        private JCDecauxItem GetJCDecauxItem(string myJsonResponse)
        {
            List<RootJCDecauxItem> r = JsonConvert.DeserializeObject<List<RootJCDecauxItem>>(myJsonResponse);
            JCDecauxItem jC = new JCDecauxItem();
            jC.root = r;
            return jC;
        }
    }
}
