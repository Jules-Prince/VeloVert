using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_for_Self_Hosted_WS
{
    internal class OSMModel
    {
        public string formatUrl(string adress)
        {
            string url = "";
            foreach (char c in adress)
            {
                if (c == ' ')
                {
                    url = url + "%";
                    url = url + "2";
                    url = url + "0";
                }
                else
                {
                    url = url + c;
                }
            };
            return url;
        }

         public async Task<string> OSMAPICall(string request)
         {
            // API : https://openrouteservice.org/
            string urlAPI = "https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf624857ddfd522faa498cb4d1d74518230dff&text=";
            request = urlAPI + request;
            //Console.WriteLine(request);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
         }
    }
}
