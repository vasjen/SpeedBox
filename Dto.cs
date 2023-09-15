namespace SpeedBox;
public record CalculationRequest(
    string cityReceiver, string citySender, int tariffId, Size size);

public record CalculationResponse(CityResponse senderCity, CityResponse receiverCity, decimal price);
public record CityResponse(int cityId, Guid fiasCity, string cityName);
public record Size(double weight, int length, int width, int height);
