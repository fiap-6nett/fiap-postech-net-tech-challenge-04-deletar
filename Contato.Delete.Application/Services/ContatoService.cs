using Contato.Atualizar.Domain.Entities;
using Contato.Delete.Application.Dtos;
using Contato.Delete.Application.Interfaces;

namespace Contato.Delete.Application.Services
{
    public class ContatoService : IContatoService
    {

        private readonly IAsyncRabbitMqProducer _producer;

        public ContatoService(IAsyncRabbitMqProducer producer)
        {
            _producer = producer;
        }

        public Task DeletarContatoAsync(Guid id)
        {
            var contato = new ContatoEntity();

            contato.SetId(id);

            _producer.EnviarMensagem(contato);

            return Task.CompletedTask;
        }
    }
}
