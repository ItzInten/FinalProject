using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Windows.Input;

namespace FinalProject;

public partial class MapPage : ContentPage
{
    private double latitude;
    private double longitude;

    public ICommand SearchCommand { get; private set; }

    private Pin myLocationPin;
    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        latitude = location.Latitude;
        longitude = location.Longitude;
    }
    public MapPage()
    {
        InitializeComponent();
        SearchCommand = new Command<string>(async (query) => await SearchLocation(query));
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync();
            }
            if (location != null)
            {
                MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
                MyMap.MoveToRegion(mapSpan);

            }
            var pin = new Pin
            {
                Label = "My Location",
                Address = $"Lat: {location.Latitude}, Lng: {location.Longitude}",
                Location = location,
                Type = PinType.Place
            };
            MyMap.Pins.Add(pin);
            MyMap.MapClicked += OnMapClicked;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        var pin = new Pin
        {
            Label = "Selected Location",
            Location = e.Location,
            Type = PinType.Place
        };

        MyMap.Pins.Clear();
        MyMap.Pins.Add(pin);
        SearchBarLocation.Unfocus();
    }

    private async Task SearchLocation(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return;

        var locations = await Geocoding.GetLocationsAsync(query);
        var location = locations?.FirstOrDefault();

        if (location != null)
        {
            MapSpan mapSpan = new MapSpan(new Location(location.Latitude, location.Longitude), 0.01, 0.01);
            MyMap.MoveToRegion(mapSpan);

            var searchPin = new Pin
            {
                Label = query,
                Address = $"Lat: {location.Latitude:F6}, Lng: {location.Longitude:F6}",
                Location = new Location(location.Latitude, location.Longitude),
                Type = PinType.SearchResult
            };

            MyMap.Pins.Clear();
            if (myLocationPin != null)
            {
                MyMap.Pins.Add(myLocationPin);
            }

            MyMap.Pins.Add(searchPin);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Location Not Found", "Try a different location.", "OK");
        }
        SearchBarLocation.Unfocus();
    }
}