

using System.Text;
using System.Text.Json;
using System.Xml;
using SpeedBox.Models;

namespace SpeedBox.Services
{
    public class CalculationService : ICalculationService
    {
       
        private readonly IHttpClientFactory _httpClientFactory;

        public CalculationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CalculationResponse> Calculate(CalculationRequest request)
        {
            var goods = new Goods ((double)request.size.width / 1000,request.size.length / 10,request.size.width / 10 ,request.size.height / 10);
            var sender = await GetCityId(Guid.Parse(request.citySender));
            var receiver = await GetCityId(Guid.Parse(request.cityReceiver));
            var client = _httpClientFactory.CreateClient("calculator");
            System.Console.WriteLine("Sizes from request:\n" + request.size.weight + "\n" + request.size.length + "\n" + request.size.width + "\n" + request.size.height + "\n");
            System.Console.WriteLine("Sizes:\n" + goods.weight + "\n" + goods.length + "\n" + goods.width + "\n" + goods.height + "\n");
            RequestToCdek requestToCdek = new RequestToCdek("1.0", sender.cityId, DateTime.Now.ToString("yyyy-MM-dd"), receiver.cityId, request.tariffId  != 0 ? request.tariffId : 480, goods);
            var json = JsonSerializer.Serialize(requestToCdek);
            var req = await client.PostAsync("http://api.cdek.ru/calculator/calculate_price_by_json.php", 
                                                    new StringContent(json, Encoding.UTF8, "application/json"));
            var response = await req.Content.ReadAsStringAsync();
            CalculationResponse calculationResponse = new CalculationResponse(sender, receiver, GetPrice(response));
        
           return calculationResponse;;
        }

        public async Task<CityResponse> GetCityId(Guid fiasCity)
        {   var client = _httpClientFactory.CreateClient("location");
            var req = await client.GetAsync($"cities?size=1&fiasGuid={fiasCity}");
            if (!req.IsSuccessStatusCode)
            {
                return null;
            }
            var response = await req.Content.ReadAsStringAsync();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            XmlNode locationNode = xmlDoc.SelectSingleNode("//Location");
            CityResponse cityResponse = new CityResponse(
                int.Parse(locationNode.Attributes["cityCode"]!.Value),
                fiasCity,
                locationNode.Attributes["cityName"]!.Value);
            return cityResponse;
        }

        private decimal GetPrice(string response)
        {
            JsonDocument jsonDoc = JsonDocument.Parse(response);
            JsonElement root = jsonDoc.RootElement;
            JsonElement priceElement = root.GetProperty("result").GetProperty("price");
            return decimal.TryParse(priceElement.ToString(), out decimal price) ? price : 0;
        }
    }
}