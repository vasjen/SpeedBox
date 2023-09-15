using SpeedBox.Models;

namespace SpeedBox.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IApiWorker _apiWorker;
        private readonly IParserService _parserService;

        public CalculationService(IApiWorker apiWorker, IParserService parserService)
        {
            _apiWorker = apiWorker;
            _parserService = parserService;
        }
        public async Task<CalculationResponse?> Calculate(CalculationRequest request)
        {
            var goods = CreateGoodsFromRequest(request);
            var sender = await GetCityId(Guid.TryParse(request.citySender, out Guid fiasCity) ? fiasCity : Guid.Empty);
            var receiver = await GetCityId(Guid.TryParse(request.cityReceiver, out fiasCity) ? fiasCity : Guid.Empty);
            if (sender == null || receiver == null)
            {
                return null;
            }
            RequestToCdek requestToCdek =  CreateRequestToCde(sender, receiver, request.tariffId, goods);
            var response = await _apiWorker.SendRequestToCalculateApi(requestToCdek);
            CalculationResponse calculationResponse = new CalculationResponse
            (
                sender, 
                receiver, 
                _parserService.GetPriceFromJson(response), 
                _parserService.GetDeliveryPeriodFromJson(response)
            );
            return calculationResponse;
        }

        public async Task<CityResponse?> GetCityId(Guid fiasCity)
        {   
           var response = await _apiWorker.SendRequestToCitiesApi(fiasCity);
           return _parserService.CreateCityResponseFromXml(response);
        }
        private Goods CreateGoodsFromRequest (CalculationRequest request)
        {
            return new Goods(
                (double)request.size.width / 1000,
                request.size.length / 10,
                request.size.width / 10 ,
                request.size.height / 10);
        }
       
        private RequestToCdek CreateRequestToCde(CityResponse sender, CityResponse receiver, int tariffId, Goods goods)
            =>  new RequestToCdek(
                    "1.0", 
                    sender.cityId, 
                    DateTime.Now.ToString("yyyy-MM-dd"), 
                    receiver.cityId, 
                    tariffId  != 0 ? tariffId : 480, 
                    goods);
        
    }
}