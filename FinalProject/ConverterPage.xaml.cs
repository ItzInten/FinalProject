using FinalProject.Services;
namespace FinalProject;

public partial class ConverterPage : ContentPage
{
    private Dictionary<string, double> CurrencyRates = new Dictionary<string, double>();
    public ConverterPage()
	{
		InitializeComponent();
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
                var ratesList = new List<KeyValuePair<string, double>>
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
                RatesListView.ItemsSource = ratesList;
                CurrencyRates = ratesList.ToDictionary(r => r.Key, r => r.Value);
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
            string fromCurrency = FromCurrencyEntry.Text?.ToUpper();  // Currency code to convert from
            string toCurrency = ToCurrencyEntry.Text?.ToUpper();      // Currency code to convert to
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
            ConversionResultLabel.Text = $"Converted Amount: {convertedAmount:F2} {toCurrency}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

}
