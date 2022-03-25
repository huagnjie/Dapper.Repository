using RabbitMQ.Client.Events;
using System;

namespace RabbitMQJie.Consumer
{
    public class RabbitMessageEntity
    {
        public EventingBasicConsumer Consumer { get; set; }
        public BasicDeliverEventArgs BasicDeliver { get; set; }
        public int Code { get; set; }
        public string Content { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }
}
