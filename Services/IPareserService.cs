using SpeedBox.Models;

namespace SpeedBox.Services
{
    public interface IParserService
    {
        CityResponse? CreateCityResponseFromXml(string xml);
        double GetPriceFromJson(string json);
        DeliveryPeriod GetDeliveryPeriodFromJson(string response);
    }
}