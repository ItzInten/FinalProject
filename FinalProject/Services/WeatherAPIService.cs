using FinalProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class WeatherAPIService
    {
        private static string API1_URL = "https://api.open-meteo.com/v1/forecast?latitude=52.5736&longitude=-0.2478&current=temperature_2m,relative_humidity_2m,is_day,precipitation&hourly=temperature_2m,relative_humidity_2m,precipitation&daily=weather_code,temperature_2m_max,temperature_2m_min,sunrise,sunset&timezone=Europe%2FLondon&past_days=1&forecast_days=3";
        private static string API2_URL = "https://geocoding-api.open-meteo.com/v1/search?name=Donetsk&count=1&language=en&format=json";
        public static async Task<RootForWeather> GetWeather(double Latitude, double Longitude)
        {
            API1_URL = String.Format("https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}&current=temperature_2m,relative_humidity_2m,is_day,precipitation&hourly=temperature_2m,relative_humidity_2m,precipitation&daily=weather_code,temperature_2m_max,temperature_2m_min,sunrise,sunset&timezone=Europe%2FLondon&past_days=1&forecast_days=3", Latitude, Longitude);
            var httpClient = new HttpClient();
            var reply = await httpClient.GetStringAsync(API1_URL);
            return JsonConvert.DeserializeObject<RootForWeather>(reply);
        }
        public static async Task<RootForWeather> GetCityDetails(string city)
        {
            API2_URL = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=en&format=json";
            var httpClient = new HttpClient();
            var reply = await httpClient.GetStringAsync(API2_URL);
            return JsonConvert.DeserializeObject<RootForWeather>(reply);
        }
    }
}