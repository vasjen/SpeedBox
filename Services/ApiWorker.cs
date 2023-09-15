using System.Text;
using System.Text.Json;
using SpeedBox.Models;

namespace SpeedBox.Services
{
    public class ApiWorker : IApiWorker
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiWorker(IHttpClientFactory httpClientFactory)
        {
               _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SendRequestToCalculateApi(RequestToCdek requestToCdek)
        {
            var json = JsonSerializer.Serialize(requestToCdek);
            var client = _httpClientFactory.CreateClient("calculator");
            var req = await client.PostAsync("", 
                new StringContent(json, Encoding.UTF8, "application/json"));
            var response = await req.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<string> SendRequestToCitiesApi(Guid fiasCity)
        {
            var client = _httpClientFactory.CreateClient("location");
            var req = await client.GetAsync($"cities?size=1&fiasGuid={fiasCity}");
            if (!req.IsSuccessStatusCode)
            {
                return null;
            }
            var response = await req.Content.ReadAsStringAsync();
            return response;
        }
    }
}