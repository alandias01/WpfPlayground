using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WpfPlayground.DesignPatterns
{
    public class MBStart
    {
        public int GetNum(int x) => 5;
        public MBStart()
        {
            var bus = new MessageBus<MessageBusMessage>();
            bus.Subscribe("stocks", (m) => Debug.WriteLine(m.Message + ", " + m.AdditionalData));
            bus.Publish("stocks", new MessageBusMessage() { Message = "Hi1", AdditionalData = "Hello1" });
            bus.Publish("stocks", new MessageBusMessage() { Message = "Hi2", AdditionalData = "Hello2" });
        }
    }

    public interface IMessagebus<T>
    {
        void Subscribe(string topic, Action<T> callback);
        void Publish(string topic, T message);
    }

    public class MessageBus<T> : IMessagebus<T> where T : IMessageBusMessage
    {
        private Dictionary<string, List<Action<T>>> Subscribers;

        public MessageBus()
        {
            Subscribers = new Dictionary<string, List<Action<T>>>();
        }

        // If topic doesn't exist, create it
        // Add handler to topic
        public void Subscribe(string topic, Action<T> callback)
        {
            List<Action<T>> callbackList;

            if (!Subscribers.TryGetValue(topic, out callbackList))
            {
                callbackList = new List<Action<T>>();
                Subscribers.Add(topic, callbackList);
            }

            callbackList.Add(callback);
        }

        // Publishes the message only if the topic exists
        public void Publish(string topic, T message)
        {
            if (Subscribers.TryGetValue(topic, out List<Action<T>> callbackList))
            {
                callbackList.ForEach(x => x(message));
            }
        }
    }

    public interface IMessageBusMessage
    {
        string Message { get; }
    }
    public class MessageBusMessage : IMessageBusMessage
    {
        public string Message { get; set; }
        public string AdditionalData { get; set; }
    }
}
