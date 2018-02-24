using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeddingPlanner;
using WeddingPlanner.Models;
 
namespace ApiCaller
{
    public class WebRequest
    {
        // The second parameter is a function that returns a Dictionary of string keys to object values.
        // If an API returned an array as its top level collection the parameter type would be "Action>"
        public static async Task GetWeddingDataAsync(string address, Action<JObject> Callback)
        {
            JObject jKey = JObject.Parse(File.ReadAllText(@"JsonData/key.json"));
            var apiKey = jKey["Key"];

            // Create a temporary HttpClient connection.
            using (var Client = new HttpClient())
            {
                try
                {
                    Client.BaseAddress = new Uri($"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={apiKey}");
                    HttpResponseMessage Response = await Client.GetAsync(""); // Make the actual API call.
                    Response.EnsureSuccessStatusCode(); // Throw error if not successful.
                    string StringResponse = await Response.Content.ReadAsStringAsync(); // Read in the response as a string.
                    
                    JObject LocationObject = JsonConvert.DeserializeObject<JObject>(StringResponse);
                    JArray ResultList = LocationObject["results"].Value<JArray>();

                    Callback(LocationObject);

                }
                catch (HttpRequestException e)
                {
                    // If something went wrong, display the error.
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
        }
    }
}
