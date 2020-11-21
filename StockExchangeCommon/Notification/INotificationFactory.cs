namespace StockExchangeNotification
{
    public interface INotificationFactory
    {
        NotificationPayload Create(Notifications notifications, string payload);
        NotificationPayload Create(byte[] byteArray);
    }
}