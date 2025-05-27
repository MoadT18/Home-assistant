using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Home_assistant
{
    public class AIResponseService
    {
        private const string API_URL = "http://pi.local:5000/prompt";
        private readonly HttpClient _httpClient;

        public AIResponseService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAIResponse(string prompt, bool includeLatestData = false)
        {
            try
            {
                string finalPrompt = prompt;

                if (includeLatestData)
                {
                    ClimateDataService climateService = new ClimateDataService();
                    var (id, temperature, co2, humidity, timestamp) = await climateService.FetchLatestClimateData();

                    if (timestamp != "Unknown")
                    {
                        finalPrompt += $" The latest measurement was recorded at {timestamp}. " +
                                       $"The CO₂ level was {co2} ppm, the temperature was {temperature}°C, and the humidity was {humidity}%.";
                    }
                }

                var content = new StringContent(JsonConvert.SerializeObject(new { prompt = finalPrompt }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(API_URL, content);
                var responseString = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                return jsonResponse.response.ToString().Replace(@"\u00b0C", "°C").Trim();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }



    }
}
