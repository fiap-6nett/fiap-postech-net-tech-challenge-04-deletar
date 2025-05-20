using Contato.Delete.Application.Dtos;
using Contato.Delete.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Contato.Delete.Web.Controllers
{
   
    [Route("api/[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly ILogger<ContatoController> _logger;
        private readonly IContatoService _contatoService;

        public ContatoController(ILogger<ContatoController> logger, 
            IContatoService contatoService)
        {
            _logger = logger;
            _contatoService = contatoService;
        }

        /// <summary>
        /// Enviar um id de contato para a fila que será deletado.
        /// </summary>
        /// <param name="id">Id do contato a ser deletado.</param>
        /// <returns>Retorna o ID.</returns>
        /// <response code="200">Contato Deletado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpDelete("[action]")]        
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletarContato(Guid id)
        {
            try
            {
                _logger.LogInformation($"Acessou {nameof(DeletarContato)}. Entrada: {id}");

                var response = new ResponseDto()
                {
                    Id = id
                };

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"Id inválido - Entrada: {id}");
                    return BadRequest(ModelState);
                }

                await _contatoService.DeletarContatoAsync(id);
                _logger.LogInformation($"Dados enviados para fila com sucesso.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falha na api DeletarContato. Erro{ex}");
                return StatusCode(500, $"Internal server error - {ex}");
            }
        }
    }
}
