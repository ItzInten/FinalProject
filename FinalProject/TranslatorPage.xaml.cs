using FinalProject.Models;
using FinalProject.Services;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace FinalProject;

public partial class TranslatorPage : ContentPage
{
    private Dictionary<string, string> languageMap = new Dictionary<string, string>
    {
        { "Arabic", "AR" },
        { "Bulgarian", "BG" },
        { "Czech", "CS" },
        { "Danish", "DA" },
        { "German", "DE" },
        { "Greek", "EL" },
        { "English", "EN" },
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
        { "Lithuanian", "LT" },
        { "Latvian", "LV" },
        { "Norwegian Bokmål", "NB" },
        { "Dutch", "NL" },
        { "Polish", "PL" },
        { "Portuguese", "PT" },
        { "Portuguese (Brazilian)", "PT-BR" },
        { "Portuguese (European)", "PT-PT" },
        { "Romanian", "RO" },
        { "Russian", "RU" },
        { "Slovak", "SK" },
        { "Slovenian", "SL" },
        { "Swedish", "SV" },
        { "Turkish", "TR" },
        { "Ukrainian", "UK" },
        { "Chinese", "ZH" },
        { "Chinese (Simplified)", "ZH-HANS" },
        { "Chinese (Traditional)", "ZH-HANT" }
    };

    public TranslatorPage()
    {
        InitializeComponent();
        languagePicker.ItemsSource = languageMap.Keys.ToList();
        languagePicker.SelectedIndex = 0;
    }

    private async void OnTranslateClicked(object sender, EventArgs e)
    {
        string text = txtInput.Text;
        string targetLanguage = languagePicker.SelectedItem.ToString();
        string languageForRequest = languageMap[targetLanguage];
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

}