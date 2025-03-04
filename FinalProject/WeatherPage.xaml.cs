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

        public string getWeatherIcon(int wmoCode, int isday)
        {
            if (wmoCode == 0 && isday == 0) return "https://openweathermap.org/img/wn/01n@2x.png";
            if (wmoCode == 0 && isday == 1) return "https://openweathermap.org/img/wn/01d@2x.png";

            if (wmoCode >= 1 && wmoCode <= 3 && isday == 0) return "https://openweathermap.org/img/wn/02n@2x.png";
            if (wmoCode >= 1 && wmoCode <= 3 && isday == 1) return "https://openweathermap.org/img/wn/02d@2x.png";

            if (wmoCode == 45 || wmoCode == 48) return "https://openweathermap.org/img/wn/50d@2x.png";

            if (wmoCode >= 51 && wmoCode <= 55) return "https://openweathermap.org/img/wn/09d@2x.png";

            if (wmoCode == 56 || wmoCode == 57) return "https://openweathermap.org/img/wn/13d@2x.png";

            if (wmoCode >= 61 && wmoCode <= 65 && isday == 0) return "https://openweathermap.org/img/wn/10n@2x.png";
            if (wmoCode >= 61 && wmoCode <= 65 && isday == 1) return "https://openweathermap.org/img/wn/10d@2x.png";

            if (wmoCode == 66 || wmoCode == 67) return "https://openweathermap.org/img/wn/13d@2x.png";

            if (wmoCode >= 71 && wmoCode <= 75) return "https://openweathermap.org/img/wn/13d@2x.png";

            if (wmoCode == 77) return "https://openweathermap.org/img/wn/13d@2x.png";

            if (wmoCode >= 80 && wmoCode <= 82) return "https://openweathermap.org/img/wn/09d@2x.png";
            if (wmoCode == 85 || wmoCode == 86) return "https://openweathermap.org/img/wn/13d@2x.png";
            if (wmoCode == 95) return "https://openweathermap.org/img/wn/11d@2x.png";
            if (wmoCode == 96 || wmoCode == 99) return "https://openweathermap.org/img/wn/11d@2x.png";

            return "https://openweathermap.org/img/wn/01d@2x.png";
        }
        protected override async void OnAppearing()
        {
            try
            {
                int currentIndex = 0;
                DateTime currentTime = DateTime.Now;
                int adjustedMinutes;
                if (currentTime.Minute >= 30)
                {
                    adjustedMinutes = 60 - currentTime.Minute;
                }
                else
                {
                    adjustedMinutes = -currentTime.Minute;
                }
                DateTime roundedTime = currentTime.AddMinutes(adjustedMinutes);
                string formattedDate = roundedTime.ToString("yyyy-MM-ddTHH:mm");
                //DateTime finalDate = formattedDate;
                base.OnAppearing();
                await GetLocation();
                var result = await WeatherAPIService.GetWeather(latitude, longitude);
                string getIconUrl = getWeatherIcon(result.current.weather_code, result.current.is_day);
                WeatherIcon.Source = getIconUrl;
                var cityName = await WeatherAPIService.GetCityDetails(Math.Round(latitude, 4), Math.Round(longitude, 4));
                CityNameLabel.Text = cityName.features[0].properties.city;
                TemperatureLabel.Text = result.current.temperature_2m.ToString() + result.current_units.temperature_2m;
                HumidityLabel.Text = result.current.relative_humidity_2m.ToString() + result.current_units.relative_humidity_2m;
                PrecipitationLabel.Text = result.current.precipitation.ToString() + "mm";
                List<HourlyWeatherData> WeatherDataCombined = new List<HourlyWeatherData>();

                for (int i = 0; i < result.hourly.time.Count; i++)
                {
                    if (formattedDate.ToString() == result.hourly.time[i])
                    {
                        currentIndex = i;
                    }
                    WeatherDataCombined.Add(new HourlyWeatherData
                    {
                        Time = result.hourly.time[i],
                        Temperature = result.hourly.temperature_2m[i],
                        Humidity = result.hourly.relative_humidity_2m[i],
                        Precipitation = result.hourly.precipitation[i],
                        WeatherImage = getWeatherIcon(result.hourly.weather_code[i], result.hourly.is_day[i])
                    });
                }
                HourlyCollectionView.ItemsSource = WeatherDataCombined;
                HourlyCollectionView.ScrollTo(currentIndex, position: ScrollToPosition.Center, animate: false);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }


    public class HourlyWeatherData
    {
        public string Time { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double Precipitation { get; set; }
        public string WeatherImage { get; set; }
        public string FormattedTime
        {
            get
            {
                DateTime parsedTime;
                if (DateTime.TryParse(Time, out parsedTime))
                {
                    return parsedTime.ToString("HH:mm");
                }
                return Time;
            }
        }
    }

}
