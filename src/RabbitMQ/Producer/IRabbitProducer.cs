using System.Collections.Generic;

namespace RabbitMQJie.Producer
{
    public interface IRabbitProducer
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="props"></param>
        /// <param name="conect"></param>
        public void Publish(string exchange, string routingKey, IDictionary<string, object> props, string conect);
    }
}
