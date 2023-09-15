namespace SpeedBox.Models
{
    public record RequestToCdek
    (
        string version, int senderCityId, string dateExecute, 
        int receiverCityId, int tariffId, Goods goods
    );
            
    }

    public record Goods
    (
        double weight, int length, int width, int height
    );
   