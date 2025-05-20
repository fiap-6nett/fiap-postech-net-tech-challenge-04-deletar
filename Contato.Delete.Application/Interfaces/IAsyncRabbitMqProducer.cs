namespace Contato.Delete.Application.Interfaces
{
    public interface IAsyncRabbitMqProducer
    {
        Task EnviarMensagem(object mensagem);
    }
}
