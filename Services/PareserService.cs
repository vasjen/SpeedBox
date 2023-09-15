using System.Text.Json;
using System.Xml;
using SpeedBox.Models;

namespace SpeedBox.Services
{
    public class ParserService : IParserService
    {
  

        public ParserService()
        {
            
        }

        public CityResponse? CreateCityResponseFromXml(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            try
            {
                XmlNode locationNode = xmlDoc.SelectSingleNode("//Location");

                if (locationNode == null)
                {
                    return null;
                }

                int cityCode;
                if (!int.TryParse(locationNode.Attributes?["cityCode"]?.Value, out cityCode))
                {
                    return null;
                }

                Guid fiasGuid;
                if (!Guid.TryParse(locationNode.Attributes?["fiasGuid"]?.Value, out fiasGuid))
                {
                   return null;
                }

                string cityName = locationNode.Attributes?["cityName"]?.Value;
                if (string.IsNullOrEmpty(cityName))
                {
                    return null;
                }

                CityResponse cityResponse = new CityResponse(cityCode, fiasGuid, cityName);
                return cityResponse;
            }
            catch (Exception ex)
            {   
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public DeliveryPeriod GetDeliveryPeriodFromJson(string response)
        {
            JsonDocument jsonDoc = JsonDocument.Parse(response);
            JsonElement root = jsonDoc.RootElement;
            JsonElement min = root.GetProperty("result").GetProperty("deliveryPeriodMin");
            JsonElement max = root.GetProperty("result").GetProperty("deliveryPeriodMax");

            return new DeliveryPeriod(min.GetInt32(), max.GetInt32());
        }

        public double GetPriceFromJson(string json)
        {
            JsonDocument jsonDoc = JsonDocument.Parse(json);
            JsonElement root = jsonDoc.RootElement;
            JsonElement priceElement = root.GetProperty("result").GetProperty("price");
            return double.TryParse(priceElement.ToString(), out double price) ? price : 0;
        }
    }
}