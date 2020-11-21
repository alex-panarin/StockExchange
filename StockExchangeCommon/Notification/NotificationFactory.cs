
using Newtonsoft.Json;
using System.Text;

namespace StockExchangeNotification
{

    public class NotificationFactory : INotificationFactory
    {
        public NotificationFactory()
        {

        }
        public NotificationPayload Create(Notifications notifications, string payload)
        {
            return new NotificationPayload
            {
                Notification = notifications,
                Payload = payload,
            };
        }
        public NotificationPayload Create(byte[] byteArray)
        {
            string payload = Encoding.UTF8.GetString(byteArray);
            
            if (!payload.Contains(":")) return null;

            if (payload.Contains("\"")) // JSON string expected
            {
                return JsonConvert.DeserializeObject<NotificationPayload>(payload);
            }
            
            return new NotificationPayload
            {
                Notification =  payload.Split(':')[0].ParseEnum<Notifications>(),
                Payload = payload.Split(':')[1]
            };
        }
    }
}
