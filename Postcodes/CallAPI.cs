using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Postcodes
{
    public class CallAPI
    {
        public static async Task<string> RunGetAsync(string str)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = client.GetAsync($"https://api.postcodes.io/postcodes/{str}").Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resp = await response.Content.ReadAsStringAsync();
                    return resp;
                }
                return response.StatusCode.ToString();
            }
        }

        public static async Task<string> RunPostAsync(PostcodeInput postcodes)
        {
            string bodyContent = JsonConvert.SerializeObject(postcodes);
            var body = new StringContent(bodyContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP Post
                var response = client.PostAsync("https://api.postcodes.io/postcodes/", body).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resp = await response.Content.ReadAsStringAsync();
                    return resp;
                }
                return response.StatusCode.ToString();
            }
        }
    }
}
