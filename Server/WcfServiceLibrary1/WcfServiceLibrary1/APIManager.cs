using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WcfServiceLibrary1
{
    internal class APIManager
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
