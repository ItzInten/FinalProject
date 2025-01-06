using FinalProject.Services;

namespace FinalProject;

public partial class ConverterPage : ContentPage
{
	public ConverterPage()
	{
		InitializeComponent();
	}

    private async Task LoadExchangeRatesAsync()
    {
        try
        {
            var result = await CurrencyAPIService.GetCoefficient();

            if (result?.Data != null)
            {
                _exchangeRates = result.Data;

                // Populate the Picker with available currencies
                ToCurrencyPicker.ItemsSource = _exchangeRates.Keys.ToList();
            }
            else
            {
                await DisplayAlert("Error", "Failed to load exchange rates.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
    private async void OnConvertButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(FromCurrencyEntry.Text) ||
                string.IsNullOrWhiteSpace(AmountEntry.Text) ||
                ToCurrencyPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            string fromCurrency = FromCurrencyEntry.Text.ToUpper();
            string toCurrency = ToCurrencyPicker.SelectedItem.ToString();
            if (!decimal.TryParse(AmountEntry.Text, out var amount))
            {
                await DisplayAlert("Error", "Invalid amount.", "OK");
                return;
            }

            // Check if the currencies exist in the rates dictionary
            if (!_exchangeRates.ContainsKey(fromCurrency) || !_exchangeRates.ContainsKey(toCurrency))
            {
                await DisplayAlert("Error", "Currency not supported.", "OK");
                return;
            }

            // Perform the conversion
            decimal fromRate = _exchangeRates[fromCurrency];
            decimal toRate = _exchangeRates[toCurrency];
            decimal convertedAmount = (amount / fromRate) * toRate;

            // Display the result
            ResultLabel.Text = $"{amount} {fromCurrency} = {convertedAmount:F2} {toCurrency}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}

// Helper class to deserialize the API response
public class CurrencyResponse
{
    public Dictionary<string, decimal> Data { get; set; }
}
}