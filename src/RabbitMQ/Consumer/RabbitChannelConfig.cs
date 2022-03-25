using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQJie.Consumer
{
    public class RabbitChannelConfig
    {
        public string ExchangeTypeName { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKeyName { get; set; }
        public IConnection Connection { get; set; }
        public EventingBasicConsumer Consumer { get; set; }

        /// <summary>
        /// 外部订阅消费者通知委托
        /// </summary>
        public Action<RabbitMessageEntity> OnReceivedCallback { get; set; }

        public RabbitChannelConfig(string exchangeType, string exchange, string queue, string routingKey)
        {
            this.ExchangeTypeName = exchangeType;
            this.ExchangeName = exchange;
            this.QueueName = queue;
            this.RoutingKeyName = routingKey;
        }

        /// <summary>
        /// 定义消息统一接收处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Receive(object sender, BasicDeliverEventArgs args)
        {
            RabbitMessageEntity body = new RabbitMessageEntity();
            try
            {
                string content = Encoding.UTF8.GetString(args.Body.ToArray());
                body.Content = content;
                body.Consumer = (EventingBasicConsumer)sender;
                body.BasicDeliver = args;
            }
            catch (Exception ex)
            {
                body.ErrorMessage = $"订阅-出错{ex.Message}";
                body.Exception = ex;
                body.Error = true;
                body.Code = 500;
            }
            OnReceivedCallback?.Invoke(body);
        }
    }
}
