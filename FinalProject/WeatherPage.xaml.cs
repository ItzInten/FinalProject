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
            //weatherpage.TimeLabel.Text = "Working";
            TimeLabel.Text = latitude.ToString();
        }
    }
}
