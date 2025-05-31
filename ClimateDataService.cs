using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Home_assistant
{
    public class ClimateDataService
    {
        private const string API_URL = "http://pi.local:8000/data";
        private readonly HttpClient _httpClient;

        public ClimateDataService()
        {
            _httpClient = new HttpClient();
        }

   
        public async Task<Dictionary<int, (double temperature, double co2, double humidity)>> FetchClimateData()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(API_URL);
                dynamic climateData = JsonConvert.DeserializeObject(response);

                var dataDict = new Dictionary<int, (double temperature, double co2, double humidity)>();
                var today = DateTime.Today;

                foreach (var entry in climateData)
                {
                    DateTime timestamp = DateTime.Parse(entry.timestamp.ToString());
                    if (timestamp.Date == today.Date) // Only today's data
                    {
                        int hour = timestamp.Hour;
                        if (!dataDict.ContainsKey(hour))
                        {
                            dataDict[hour] = ((double)entry.temperature, (double)entry.co2, (double)entry.humidity);
                        }
                    }
                }
                return dataDict;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching climate data: " + ex.Message);
                return null;
            }
        }

  
        public async Task<(int id, double temperature, double co2, double humidity, string timestamp)> FetchLatestClimateData()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(API_URL);
                dynamic data = JsonConvert.DeserializeObject(response);

                var latestEntry = data[0];
                int id = (int)latestEntry.id;
                double temp = (double)latestEntry.temperature;
                double co2 = (double)latestEntry.co2;
                double hum = (double)latestEntry.humidity;
                DateTime ts = DateTime.Parse(latestEntry.timestamp.ToString());
                string formattedTime = ts.ToString("HH:mm");

                return (id, temp, co2, hum, formattedTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching latest climate data: " + ex.Message);
                return (0, 0, 0, 0, "Unknown");
            }
        }
        }
}
