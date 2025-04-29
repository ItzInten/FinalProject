using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class TranslatorAPIService
    {
        private const string ApiUrl = "https://api-free.deepl.com/v2/translate";
        private const string ApiKey = "APIKEY";
        public static async Task<string> TranslateTextAsync(string text, string targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(text)) return "Enter text to translate.";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {ApiKey}");

                var requestBody = new
                {
                    text = new string[] { text },
                    target_lang = targetLanguage.ToUpper()
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(ApiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    var translatedText = doc.RootElement.GetProperty("translations")[0].GetProperty("text").GetString();
                    return translatedText;
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
        }
    }
}
