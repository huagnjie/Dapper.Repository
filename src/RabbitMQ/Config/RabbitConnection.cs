using RabbitMQ.Client;
using System.Collections.Generic;

namespace RabbitMQJie.Config
{
    public class RabbitConnection
    {
        private readonly RabbitOption _config;
        private IConnection _connection = null;

        public RabbitConnection(RabbitOption conifg)
        {
            _config = conifg;
        }

        public IConnection GetConnection()
        {
            if (_connection == null)
            {
                if (string.IsNullOrEmpty(_config.Address))
                {
                    ConnectionFactory factory = new ConnectionFactory();
                    factory.HostName = _config.HostName;
                    factory.Port = _config.Port;
                    factory.UserName = _config.UserName;
                    factory.Password = _config.PassWord;
                    factory.VirtualHost = _config.VirtualHost;
                    _connection = factory.CreateConnection();
                }
                else
                {
                    ConnectionFactory factory = new ConnectionFactory();
                    factory.UserName = _config.UserName;
                    factory.Password = _config.PassWord;
                    factory.VirtualHost = _config.VirtualHost;
                    var address = _config.Address;
                    List<AmqpTcpEndpoint> endpoints = new List<AmqpTcpEndpoint>();
                    var addressList = address.Split(",");
                    foreach (var endpoint in addressList)
                    {
                        endpoints.Add(new AmqpTcpEndpoint(endpoint.Split(":")[0], int.Parse(endpoint.Split(":")[1])));
                    }
                    _connection = factory.CreateConnection(endpoints);
                }
            }
            return _connection;
        }
    }
}
