using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeetrootTgBot.Services
{
    public class Location
    {
        public int Woeid { get; set; }
    }

    public class WeatherInfo
    {
        public string Title { get; set; }
        public IEnumerable<Weather> ConsolidatedWeather { get; set; }
    }

    public class Weather
    {
        public DateTime ApplicableDate { get; set; }
        public decimal MaxTemp { get; set; }
        public decimal MinTemp { get; set; }
        public decimal TheTemp { get; set; }
    }

    public interface IWeatherApiClient
    {
        Task<IEnumerable<Location>> GetLocationsByQuery(string query,
            CancellationToken cancellationToken = default);

        Task<WeatherInfo> GetWeatherByWoeid(int woeid, CancellationToken cancellationToken = default);
    }

    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settings;

        public WeatherApiClient(HttpClient client, JsonSerializerSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task<IEnumerable<Location>> GetLocationsByQuery(string query,
            CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/location/search/?query={query}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Weather API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<IEnumerable<Location>>(content, _settings);

            return result;
        }

        public async Task<WeatherInfo> GetWeatherByWoeid(int woeid, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/location/{woeid}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Weather API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<WeatherInfo>(content, _settings);

            return result;
        }
    }
}
