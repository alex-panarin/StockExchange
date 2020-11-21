using Newtonsoft.Json;
using System;
using System.Text;

namespace StockExchangeNotification
{
    public static class NotificationHelper
    {
        public static TEnum ParseEnum<TEnum>(this string value) where TEnum : Enum
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value");

            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
    }
    public enum Notifications
    {
        Error = 1,
        Connect,
        Disconnect,
        Notify
    }

    public class NotificationPayload 
    {
        public Notifications Notification { get; set; }
        public string Payload { get; set; }
        public override string ToString()
        {
            return $"{Notification}:{Payload}";
        }
        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }
        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public byte[] ToJsonByteArray()
        {
            return Encoding.UTF8.GetBytes(ToJsonString());
        }
    }

}
