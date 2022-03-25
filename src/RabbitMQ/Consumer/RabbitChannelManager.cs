using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQJie.Config;
using System.Collections;
using System.Collections.Generic;

namespace RabbitMQJie.Consumer
{
    public class RabbitChannelManager
    {
        public RabbitConnection _connection { get; set; }
        public RabbitChannelManager(RabbitConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// 创建接收消息的通道
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public RabbitChannelConfig CreateReceiveChannel(string exchangeType,string exchange,string queue,string routingKey,IDictionary<string ,object> arguments = null)
        {
            IModel model = this.CreateModel(exchangeType, exchange, queue, routingKey, arguments);
            //创建消费者
            EventingBasicConsumer consumer = this.CreateConsumer(model,queue);
            RabbitChannelConfig channel = new RabbitChannelConfig(exchangeType,exchange,queue,routingKey);
            //绑定事件
            consumer.Received += channel.Receive;
            return channel;
        }

        /// <summary>
        /// 创建一个通道 包含交换机/队列/路由，并建立绑定关系
        /// </summary>
        /// <param name="exchangeType">交换机类型:Topic、Direct、Fanout</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由名称</param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private IModel CreateModel(string exchangeType,string exchange,string queue,string routingKey,IDictionary<string,object> arguments)
        {
            exchangeType = string.IsNullOrWhiteSpace(exchangeType) ? "default" : exchangeType;
            //创建通道
            IModel model = _connection.GetConnection().CreateModel();
            //每次只接收一条
            model.BasicQos(0, 1, false);
            //创建队列
            model.QueueDeclare(queue,true,false,false,arguments);
            //创建交换机
            model.ExchangeDeclare(exchange,exchangeType);
            model.QueueBind(queue,exchange,routingKey);
            return model;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="model"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        private EventingBasicConsumer CreateConsumer(IModel model,string queue)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(model);
            model.BasicConsume(queue,false,consumer);
            return consumer;
        }
    }
}
