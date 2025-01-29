using FinalProject.Models;
using FinalProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public partial class WeatherPage : ContentPage
    {

        private double latitude;
        private double longitude;
        public WeatherPage()
        {
            InitializeComponent();
        }
        public async Task GetLocation()
        {
            var location = await Geolocation.GetLocationAsync();
            latitude = location.Latitude;
            longitude = location.Longitude;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetLocation();
            var result = await WeatherAPIService.GetWeather(latitude, longitude);
            var cityName = await WeatherAPIService.GetCityDetails(Math.Round(latitude, 4),Math.Round(longitude, 4));
            CityNameLabel.Text = cityName.features[0].properties.city;
            TemperatureLabel.Text = result.current.temperature_2m.ToString()+result.current_units.temperature_2m;
            HumidityLabel.Text = result.current.relative_humidity_2m.ToString()+result.current_units.relative_humidity_2m;
            PrecipitationLabel.Text = result.current.precipitation.ToString()+"mm";
            List<HourlyWeatherData> WeatherDataCombined = new List<HourlyWeatherData>();

            for (int i = 0; i < result.hourly.time.Count; i++)
            {
                WeatherDataCombined.Add(new HourlyWeatherData
                {
                    Time = result.hourly.time[i],
                    Temperature = result.hourly.temperature_2m[i],
                    Humidity = result.hourly.relative_humidity_2m[i],
                    Precipitation = result.hourly.precipitation[i]
                });
            }
            HourlyCollectionView.ItemsSource = WeatherDataCombined;

        }
    }

    public class HourlyWeatherData
    {
        public string Time { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double Precipitation { get; set; }
    }

}
