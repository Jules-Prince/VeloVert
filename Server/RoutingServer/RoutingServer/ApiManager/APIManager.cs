using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace RoutingServer
{
    public class ApiManager
    {
        /**
         * This class is the interface to the APIs. 
         * It is the class that executes the requests.
         */

        /**
         * This method corrects the gaps with the html code for the requests.
         */
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

        /**
         * This method changes the ',' to '.' to put the doubles in the right format. 
         * It is an adapter for the values to put in the queries. 
         */
        public string correctPostionFormat(double val)
        {
            string s = "" + val;
            string result = "";

            foreach (char c in s)
            {
                if (c == ',')
                {
                    result = result + ".";
                }
                else
                {
                    result = result + c;
                }
            }

            return result;
        }

        /**
         * This function makes the different requests to the APIs.
         */
        public async Task<string> APICall(string urlAPI, string param)
        {
            string url = urlAPI + param;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
