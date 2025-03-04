using FinalProject.Services;
namespace FinalProject;

public partial class MainPageOfTheProject : TabbedPage
{
    private double latitude;
    private double longitude;
    private Dictionary<string, double> CurrencyRates = new Dictionary<string, double>();
    private List<KeyValuePair<string, double>> ratesList = new List<KeyValuePair<string, double>>();
    private Dictionary<string, string> countryCodeToCurrencyMap = new Dictionary<string, string>
    {
        // Countries using AUD (Australian Dollar)
        { "AU", "AUD" },  // Australia

        // Countries using BGN (Bulgarian Lev)
        { "BG", "BGN" },  // Bulgaria

        // Countries using BRL (Brazilian Real)
        { "BR", "BRL" },  // Brazil

        // Countries using CAD (Canadian Dollar)
        { "CA", "CAD" },  // Canada

        // Countries using CHF (Swiss Franc)
        { "CH", "CHF" },  // Switzerland

        // Countries using CNY (Chinese Yuan)
        { "CN", "CNY" },  // China

        // Countries using CZK (Czech Koruna)
        { "CZ", "CZK" },  // Czech Republic

        // Countries using DKK (Danish Krone)
        { "DK", "DKK" },  // Denmark

        // Countries using EUR (Euro)
        { "DE", "EUR" },  // Germany
        { "FR", "EUR" },  // France
        { "IT", "EUR" },  // Italy
        { "ES", "EUR" },  // Spain
        { "NL", "EUR" },  // Netherlands
        { "GR", "EUR" },  // Greece
        { "PT", "EUR" },  // Portugal
        { "IE", "EUR" },  // Ireland
        { "BE", "EUR" },  // Belgium
        { "FI", "EUR" },  // Finland
        { "AT", "EUR" },  // Austria
        { "LU", "EUR" },  // Luxembourg
        { "MT", "EUR" },  // Malta
        { "CY", "EUR" },  // Cyprus
        { "SI", "EUR" },  // Slovenia
        { "SK", "EUR" },  // Slovakia
        { "EE", "EUR" },  // Estonia
        { "LV", "EUR" },  // Latvia
        { "LT", "EUR" },  // Lithuania
        // More countries that use EUR can be added here

        // Countries using GBP (British Pound Sterling)
        { "GB", "GBP" },  // United Kingdom

        // Countries using HKD (Hong Kong Dollar)
        { "HK", "HKD" },  // Hong Kong

        // Countries using HRK (Croatian Kuna)
        { "HR", "HRK" },  // Croatia

        // Countries using HUF (Hungarian Forint)
        { "HU", "HUF" },  // Hungary

        // Countries using IDR (Indonesian Rupiah)
        { "ID", "IDR" },  // Indonesia

        // Countries using INR (Indian Rupee)
        { "IN", "INR" },  // India

        // Countries using JPY (Japanese Yen)
        { "JP", "JPY" },  // Japan

        // Countries using KRW (South Korean Won)
        { "KR", "KRW" },  // South Korea

        // Countries using MXN (Mexican Peso)
        { "MX", "MXN" },  // Mexico

        // Countries using NOK (Norwegian Krone)
        { "NO", "NOK" },  // Norway

        // Countries using NZD (New Zealand Dollar)
        { "NZ", "NZD" },  // New Zealand

        // Countries using PLN (Polish Zloty)
        { "PL", "PLN" },  // Poland

        // Countries using RON (Romanian Leu)
        { "RO", "RON" },  // Romania

        // Countries using RUB (Russian Ruble)
        { "RU", "RUB" },  // Russia

        // Countries using SEK (Swedish Krona)
        { "SE", "SEK" },  // Sweden

        // Countries using SGD (Singapore Dollar)
        { "SG", "SGD" },  // Singapore

        // Countries using THB (Thai Baht)
        { "TH", "THB" },  // Thailand

        // Countries using TRY (Turkish Lira)
        { "TR", "TRY" },  // Turkey

        // Countries using USD (United States Dollar)
        { "US", "USD" },  // United States
        { "PR", "USD" },  // Puerto Rico (territory of the United States)
        { "FM", "USD" },  // Federated States of Micronesia

        // Countries using ZAR (South African Rand)
        { "ZA", "ZAR" },  // South Africa
    };

    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        latitude = location.Latitude;
        longitude = location.Longitude;
    }
    private async Task UpdateToCurrencyPicker()
    {
        try
        {
            await GetLocation();
            ToCurrencyPicker.ItemsSource = ratesList.Select(item => item.Key).ToList();  // Populate the currency picker with the list of currencies
            var countryName = await WeatherAPIService.GetCityDetails(Math.Round(latitude, 4), Math.Round(longitude, 4));
            string countryCode = countryName.features[0].properties.country_code;

            // Find the corresponding currency code from the countryCodeToCurrencyMap using the country code
            var currencyCode = countryCodeToCurrencyMap
                                .Where(x => x.Key.Equals(countryCode, StringComparison.OrdinalIgnoreCase))  // Match country code
                                .Select(x => x.Value)  // Get the currency code
                                .FirstOrDefault();  // Use FirstOrDefault to get the first matching currency code

            if (!string.IsNullOrEmpty(currencyCode))  // If a currency code is found
            {
                // Select the currency in the picker
                var selectedCurrency = ratesList.FirstOrDefault(x => x.Key.Equals(currencyCode, StringComparison.OrdinalIgnoreCase)).Key;

                if (selectedCurrency != null)
                {
                    ToCurrencyPicker.SelectedItem = selectedCurrency;  // Set the selected item in the currency picker
                }
            }


        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No currency rates available.", "OK");
        }
    }
    public MainPageOfTheProject()
    {
        InitializeComponent();
        Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var result = await CurrencyAPIService.GetCoefficient();

            if (result?.data != null)
            {
                // Convert dictionary to a list of key-value pairs
                ratesList = new List<KeyValuePair<string, double>>
                {
                new KeyValuePair<string, double>("AUD", result.data.AUD),
                new KeyValuePair<string, double>("BGN", result.data.BGN),
                new KeyValuePair<string, double>("BRL", result.data.BRL),
                new KeyValuePair<string, double>("CAD", result.data.CAD),
                new KeyValuePair<string, double>("CHF", result.data.CHF),
                new KeyValuePair<string, double>("CNY", result.data.CNY),
                new KeyValuePair<string, double>("CZK", result.data.CZK),
                new KeyValuePair<string, double>("DKK", result.data.DKK),
                new KeyValuePair<string, double>("EUR", result.data.EUR),
                new KeyValuePair<string, double>("GBP", result.data.GBP),
                new KeyValuePair<string, double>("HKD", result.data.HKD),
                new KeyValuePair<string, double>("HRK", result.data.HRK),
                new KeyValuePair<string, double>("HUF", result.data.HUF),
                new KeyValuePair<string, double>("IDR", result.data.IDR),
                new KeyValuePair<string, double>("INR", result.data.INR),
                new KeyValuePair<string, double>("JPY", result.data.JPY),
                new KeyValuePair<string, double>("KRW", result.data.KRW),
                new KeyValuePair<string, double>("MXN", result.data.MXN),
                new KeyValuePair<string, double>("NOK", result.data.NOK),
                new KeyValuePair<string, double>("NZD", result.data.NZD),
                new KeyValuePair<string, double>("PLN", result.data.PLN),
                new KeyValuePair<string, double>("RON", result.data.RON),
                new KeyValuePair<string, double>("RUB", result.data.RUB),
                new KeyValuePair<string, double>("SEK", result.data.SEK),
                new KeyValuePair<string, double>("SGD", result.data.SGD),
                new KeyValuePair<string, double>("THB", result.data.THB),
                new KeyValuePair<string, double>("TRY", result.data.TRY),
                new KeyValuePair<string, double>("USD", result.data.USD),
                new KeyValuePair<string, double>("ZAR", result.data.ZAR)
                };

                // Bind the list to the ListView
                //RatesListView.ItemsSource = ratesList;
                FromCurrencyPicker.ItemsSource = ratesList.Select(item => item.Key).ToList();
                ToCurrencyPicker.ItemsSource = ratesList.Select(item => item.Key).ToList();
                CurrencyRates = ratesList.ToDictionary(r => r.Key, r => r.Value);
                await UpdateToCurrencyPicker();
            }
            else
            {
                await DisplayAlert("Error", "No currency rates available.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnShowResultClicked(object sender, EventArgs e)
    {
        try
        {
            // Get the user input
            string fromCurrency = FromCurrencyPicker.SelectedItem?.ToString();  // Currency code to convert from
            string toCurrency = ToCurrencyPicker.SelectedItem?.ToString();   // Currency code to convert to
            double amount = Convert.ToDouble(AmountEntry.Text);       // Amount to convert

            if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
            {
                await DisplayAlert("Error", "Please enter both source and target currency codes.", "OK");
                return;
            }

            if (!CurrencyRates.ContainsKey(fromCurrency) || !CurrencyRates.ContainsKey(toCurrency))
            {
                await DisplayAlert("Error", "Invalid currency code(s). Please enter valid currency codes.", "OK");
                return;
            }

            // Get the exchange rates for the selected currencies
            double fromRate = CurrencyRates[fromCurrency];
            double toRate = CurrencyRates[toCurrency];

            // Convert the amount using the exchange rates
            double convertedAmount = (amount / fromRate) * toRate;

            // Display the result
            ResultLabel.Text = $"{convertedAmount:F2} {toCurrency}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private void OnSwapCurrencies(object sender, EventArgs e)
    {
        // Get the selected values from both pickers
        var fromCurrency = FromCurrencyPicker.SelectedItem;
        var toCurrency = ToCurrencyPicker.SelectedItem;

        // Swap the values
        FromCurrencyPicker.SelectedItem = toCurrency;
        ToCurrencyPicker.SelectedItem = fromCurrency;
    }

}
