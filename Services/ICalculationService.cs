using SpeedBox.Models;

namespace SpeedBox.Services
{
    public interface ICalculationService
    {
        Task<CalculationResponse?> Calculate(CalculationRequest request);
        Task<CityResponse?> GetCityId(Guid fiasCity);
    }
}