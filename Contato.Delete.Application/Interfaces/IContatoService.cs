using Contato.Delete.Application.Dtos;

namespace Contato.Delete.Application.Interfaces
{
    public interface IContatoService
    {
        Task DeletarContatoAsync(DeletarContatoDto dto);
    }
}
