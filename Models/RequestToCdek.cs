namespace SpeedBox.Models
{
    public record RequestToCdek
    (
        string version, int senderCityId, string dateExecute, 
        int receiverCityId, int tariffId, Goods goods
    );     

    public record Goods
    (
        double weight, int length, int width, int height
    );

    public record CalculationRequest
    (
        string cityReceiver, string citySender, int tariffId, Size size
    );

    public record CalculationResponse
    (
        CityResponse senderCity, CityResponse receiverCity, double price, DeliveryPeriod deliveryPeriod
    );
    public record DeliveryPeriod
    (
        int deliveryPeriodMin, int deliveryPeriodMax
    );
    public record CityResponse
    (
        int cityId, Guid fiasCity, string cityName
    );
    public record Size
    (
        double weight, int length, int width, int height
    );
}
   