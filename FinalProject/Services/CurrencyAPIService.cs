using FinalProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class CurrencyAPIService
    {
        private const string API_URL = "https://api.freecurrencyapi.com/v1/latest?apikey=APIKEY";
        public static async Task <Root> GetCoefficient()
        {
            var httpClient = new HttpClient();
            var reply = await httpClient.GetStringAsync(API_URL);
            return JsonConvert.DeserializeObject<Root>(reply);
        }
    }
}
