using FinalProject.Models;
using FinalProject.Services;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using CommunityToolkit.Maui.Core.Platform;
using Microsoft.Maui.Media;
using System.Globalization;


namespace FinalProject;

public partial class TranslatorPage : ContentPage
{
    string languageForRequest = "";
    string text = "";
    private double latitude;
    private double longitude;
    private Dictionary<string, string> languageMap = new Dictionary<string, string>
    {
        { "Arabic", "AR" },
        { "Bulgarian", "BG" },
        { "Czech", "CS" },
        { "Danish", "DA" },
        { "German", "DE" },
        { "Greek", "EL" },
        { "English (British)", "EN-GB" },
        { "English (American)", "EN-US" },
        { "Spanish", "ES" },
        { "Estonian", "ET" },
        { "Finnish", "FI" },
        { "French", "FR" },
        { "Hungarian", "HU" },
        { "Indonesian", "ID" },
        { "Italian", "IT" },
        { "Japanese", "JA" },
        { "Korean", "KO" },
        { "Latvian", "LV" },
        { "Norwegian Bokmål", "NB" },
        { "Dutch", "NL" },
        { "Polish", "PL" },
        { "Portuguese (Brazilian)", "PT-BR" },
        { "Portuguese (European)", "PT-PT" },
        { "Romanian", "RO" },
        { "Russian", "RU" },
        { "Slovak", "SK" },
        { "Swedish", "SV" },
        { "Turkish", "TR" },
        { "Ukrainian", "UK" },
        { "Chinese", "ZH" }
    };

    private List<Tuple<string, string>> countryToLanguageMap = new List<Tuple<string, string>>
    {
        // Arabic
        Tuple.Create("AE", "AR"), // United Arab Emirates
        Tuple.Create("SA", "AR"), // Saudi Arabia
        Tuple.Create("EG", "AR"), // Egypt
        Tuple.Create("SY", "AR"), // Syria
        Tuple.Create("IQ", "AR"), // Iraq
        Tuple.Create("JO", "AR"), // Jordan
        Tuple.Create("LB", "AR"), // Lebanon
        Tuple.Create("LY", "AR"), // Libya
        Tuple.Create("MA", "AR"), // Morocco
        Tuple.Create("OM", "AR"), // Oman
        Tuple.Create("QA", "AR"), // Qatar
        Tuple.Create("KW", "AR"), // Kuwait
        Tuple.Create("BH", "AR"), // Bahrain
        Tuple.Create("DZ", "AR"), // Algeria
        Tuple.Create("TN", "AR"), // Tunisia
        Tuple.Create("PS", "AR"), // Palestine
        Tuple.Create("SD", "AR"), // Sudan

        // Bulgarian
        Tuple.Create("BG", "BG"), // Bulgaria

        // Czech
        Tuple.Create("CZ", "CS"), // Czech Republic

        // Danish
        Tuple.Create("DK", "DA"), // Denmark

        // German
        Tuple.Create("DE", "DE"), // Germany
        Tuple.Create("AT", "DE"), // Austria
        Tuple.Create("CH", "DE"), // Switzerland
        Tuple.Create("LI", "DE"), // Liechtenstein
        Tuple.Create("BE", "DE"), // Belgium (German-speaking region)

        // Greek
        Tuple.Create("GR", "EL"), // Greece
        Tuple.Create("CY", "EL"), // Cyprus

        // English (British)
        Tuple.Create("GB", "EN-GB"), // United Kingdom

        // English (American)
        Tuple.Create("US", "EN-US"), // United States
        Tuple.Create("CA", "EN-US"), // Canada (English-speaking regions)
        Tuple.Create("AU", "EN-US"), // Australia
        Tuple.Create("NZ", "EN-US"), // New Zealand
        Tuple.Create("IE", "EN-GB"), // Ireland (English-speaking region)

        // Spanish
        Tuple.Create("ES", "ES"), // Spain
        Tuple.Create("MX", "ES"), // Mexico
        Tuple.Create("AR", "ES"), // Argentina
        Tuple.Create("CO", "ES"), // Colombia
        Tuple.Create("PE", "ES"), // Peru
        Tuple.Create("CL", "ES"), // Chile
        Tuple.Create("EC", "ES"), // Ecuador
        Tuple.Create("VE", "ES"), // Venezuela
        Tuple.Create("DO", "ES"), // Dominican Republic
        Tuple.Create("GT", "ES"), // Guatemala

        // Estonian
        Tuple.Create("EE", "ET"), // Estonia

        // Finnish
        Tuple.Create("FI", "FI"), // Finland

        // French
        Tuple.Create("FR", "FR"), // France
        Tuple.Create("CA", "FR"), // Canada (French-speaking regions)
        Tuple.Create("BE", "FR"), // Belgium (French-speaking region)
        Tuple.Create("CH", "FR"), // Switzerland (French-speaking regions)
        Tuple.Create("LU", "FR"), // Luxembourg
        Tuple.Create("MC", "FR"), // Monaco

        // Hungarian
        Tuple.Create("HU", "HU"), // Hungary

        // Indonesian
        Tuple.Create("ID", "ID"), // Indonesia

        // Italian
        Tuple.Create("IT", "IT"), // Italy
        Tuple.Create("CH", "IT"), // Switzerland (Italian-speaking regions)

        // Japanese
        Tuple.Create("JP", "JA"), // Japan

        // Korean
        Tuple.Create("KR", "KO"), // South Korea

        // Latvian
        Tuple.Create("LV", "LV"), // Latvia

        // Norwegian Bokmål
        Tuple.Create("NO", "NB"), // Norway

        // Dutch
        Tuple.Create("NL", "NL"), // Netherlands
        Tuple.Create("BE", "NL"), // Belgium (Dutch-speaking region)

        // Polish
        Tuple.Create("PL", "PL"), // Poland

        // Portuguese (Brazilian)
        Tuple.Create("BR", "PT-BR"), // Brazil

        // Portuguese (European)
        Tuple.Create("PT", "PT-PT"), // Portugal

        // Romanian
        Tuple.Create("RO", "RO"), // Romania

        // Russian
        Tuple.Create("RU", "RU"), // Russia
        Tuple.Create("BY", "RU"), // Belarus
        Tuple.Create("KZ", "RU"), // Kazakhstan

        // Slovak
        Tuple.Create("SK", "SK"), // Slovakia

        // Swedish
        Tuple.Create("SE", "SV"), // Sweden

        // Turkish
        Tuple.Create("TR", "TR"), // Turkey
        Tuple.Create("CY", "TR"), // Cyprus (Turkish-speaking region)

        // Ukrainian
        Tuple.Create("UA", "UK"), // Ukraine

        // Chinese
        Tuple.Create("CN", "ZH"), // China
        Tuple.Create("TW", "ZH"), // Taiwan
        Tuple.Create("SG", "ZH"), // Singapore
        Tuple.Create("MY", "ZH")  // Malaysia
    };



    public TranslatorPage()
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
        await UpdateLanguagePicker();
        base.OnAppearing();
    }

    private async Task UpdateLanguagePicker()
    {
        try
        {
            await GetLocation();
            languagePicker.ItemsSource = languageMap.Keys.ToList();
            var countryName = await WeatherAPIService.GetCityDetails(Math.Round(latitude, 4), Math.Round(longitude, 4));
            string countryCode = countryName.features[0].properties.country_code;
            var languageCodes = countryToLanguageMap // Find the corresponding language from the countryToLanguageMap using the country code
                                    .Where(x => x.Item1.Equals(countryCode, StringComparison.OrdinalIgnoreCase))  //country code
                                    .Select(x => x.Item2)  //language code
                                    .ToList();
            if (languageCodes.Any())// selecting the first available language in the picker
            {
                var languageNames = languageCodes // Find the language names for the matched language codes
                                        .Select(code => languageMap.FirstOrDefault(x => x.Value.Equals(code, StringComparison.OrdinalIgnoreCase)).Key)
                                        .ToList();  // get the language name
                var selectedLanguage = languageNames.FirstOrDefault(language => languagePicker.Items.Contains(language));

                if (selectedLanguage != null)
                {
                    languagePicker.SelectedItem = selectedLanguage;
                }
            }
        }
        catch (Exception ex)
        {
            languagePicker.SelectedItem = "EN-GB";
            await DisplayAlert("Language model was not recognised automatically", $"Voiceover language set to English (Great Britain)", "OK");
        }
    }



    private async void OnTranslateClicked(object sender, EventArgs e)
    {
        try
        {
            text = txtInput.Text;
            string targetLanguage = languagePicker.SelectedItem.ToString();
            languageForRequest = languageMap[targetLanguage];
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(languageForRequest))
            {
                lblTranslation.Text = "Please enter text and select a language.";
                return;
            }

            string translatedText = await TranslatorAPIService.TranslateTextAsync(text, languageForRequest);
            if (translatedText != null)
            {
                lblTranslation.Text = translatedText;
            }
            else
            {
                lblTranslation.Text = "Translation failed.";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
        await KeyboardExtensions.HideKeyboardAsync(txtInput);
    }

    private async void SpeakText(object sender, EventArgs e)
    {
        string textToSpeak = lblTranslation.Text;
        if (!string.IsNullOrWhiteSpace(textToSpeak))
        {
            await SpeakWithCorrectVoice(textToSpeak, languageForRequest);
        }

    }

    public async Task SpeakWithCorrectVoice(string text, string targetLanguageCode)
    {
        var voices = await TextToSpeech.GetLocalesAsync();

        var selectedVoice = voices.FirstOrDefault(v => {
            if (targetLanguageCode.Contains('-'))
            {
                var targetParts = targetLanguageCode.Split('-'); // Split into language and country
                var language = targetParts[0]; //before -
                var country = targetParts[1]; //after -

                return v.Language.Equals(language, StringComparison.OrdinalIgnoreCase) &&
                       v.Country.Equals(country, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return v.Language.Equals(targetLanguageCode, StringComparison.OrdinalIgnoreCase);
            }
        });

        try
        {
            var settings = new SpeechOptions { Locale = selectedVoice };
            await TextToSpeech.SpeakAsync(text, settings);
        }
        
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"The language is not supported for voiceover", "OK");
        }

    }

}