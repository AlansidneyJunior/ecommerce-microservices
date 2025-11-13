using Estoque.Application.DTOs;
using Estoque.Application.Interfaces;
using Estoque.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(
            IProdutoService produtoService,
            ILogger<ProdutosController> logger)
        {
            _produtoService = produtoService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetTodos()
        {
            _logger.LogInformation("Buscando todos os produtos");
            var produtos = await _produtoService.ObterTodosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProdutoDto>> GetPorId(int id)
        {
            _logger.LogInformation("Buscando produto com ID: {ProdutoId}", id);

            try
            {
                var produto = await _produtoService.ObterPorIdAsync(id);
                return Ok(produto);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Produto não encontrado: {ProdutoId}", id);
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProdutoDto>> Criar([FromBody] CriarProdutoDto dto)
        {
            _logger.LogInformation("Criando novo produto: {ProdutoNome}", dto.Nome);

            try
            {
                var produto = await _produtoService.CriarAsync(dto);

                return CreatedAtAction(
                    nameof(GetPorId),
                    new { id = produto.Id },
                    produto
                );
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao criar produto");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProdutoDto>> Atualizar(int id, [FromBody] AtualizarProdutoDto dto)
        {
            _logger.LogInformation("Atualizando produto: {ProdutoId}", id);

            try
            {
                var produto = await _produtoService.AtualizarAsync(id, dto);
                return Ok(produto);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Produto não encontrado: {ProdutoId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar produto");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Deletar(int id)
        {
            _logger.LogInformation("Deletando produto: {ProdutoId}", id);

            try
            {
                await _produtoService.DeletarAsync(id);
                return NoContent(); // 204 - Sucesso sem conteúdo no body
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Produto não encontrado: {ProdutoId}", id);
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/estoque")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AtualizarEstoque(int id, [FromBody] AtualizarEstoqueDto dto)
        {
            _logger.LogInformation(
                "Atualizando estoque do produto {ProdutoId} em {Quantidade}",
                id,
                dto.Quantidade
            );

            try
            {
                await _produtoService.AtualizarEstoqueAsync(id, dto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Produto não encontrado: {ProdutoId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erro ao atualizar estoque");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/disponibilidade")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> VerificarDisponibilidade(int id, [FromQuery] int quantidade)
        {
            _logger.LogInformation(
                "Verificando disponibilidade do produto {ProdutoId} para quantidade {Quantidade}",
                id,
                quantidade
            );

            var disponivel = await _produtoService.VerificarDisponibilidadeAsync(id, quantidade);
            return Ok(new { disponivel, produtoId = id, quantidadeSolicitada = quantidade });
        }
    }
}