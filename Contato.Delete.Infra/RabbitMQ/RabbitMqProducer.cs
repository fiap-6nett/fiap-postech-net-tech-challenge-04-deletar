using Contato.Delete.Application.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Contato.Delete.Infra.RabbitMQ
{
    public class RabbitMqProducer : IAsyncRabbitMqProducer
    {
        private readonly RabbitMQSettings _settings;

        public RabbitMqProducer(IOptions<RabbitMQSettings> options)
        {
            _settings = options.Value;
        }
        public async Task EnviarMensagem(object mensagem)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var json = JsonSerializer.Serialize(mensagem);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: _settings.QueueName,
                body: body
                );
        }
    }
}
