using SpeedBox.Models;

namespace SpeedBox.Services
{
    public interface IApiWorker
    {
        Task<string> SendRequestToCalculateApi(RequestToCdek requestToCdek);
        Task<string> SendRequestToCitiesApi(Guid fiasCity);
    }
}