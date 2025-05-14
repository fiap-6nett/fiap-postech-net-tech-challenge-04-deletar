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
        /// Enviar um contato para a fila que será deletado.
        /// </summary>
        /// <param name="dto">Dados do contato a serem deletados.</param>
        /// <returns>Retorna o ID.</returns>
        /// <response code="200">Contato Deletado com sucesso</response>
        /// <response code="400">Dados inválidos</response>

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletarContato([FromRoute] DeletarContatoDto dto,  Guid id)
        {
            try
            {
                _logger.LogInformation($"Acessou {nameof(DeletarContato)}. Entrada: {dto}");

                var response = new ResponseDto()
                {
                    Id = dto.Id
                };

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"Dados inválidos - Entrada: {dto}");
                    return BadRequest(ModelState);
                }

                await _contatoService.DeletarContatoAsync(dto);
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
