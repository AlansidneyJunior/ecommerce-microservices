using Estoque.Application.DTOs;

namespace Estoque.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoDto>> ObterTodosAsync();
        Task<ProdutoDto> ObterPorIdAsync(int id);
        Task<ProdutoDto> CriarAsync(CriarProdutoDto dto);
        Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto dto);
        Task DeletarAsync(int id);
        Task AtualizarEstoqueAsync(int id, AtualizarEstoqueDto dto);
        Task<bool> VerificarDisponibilidadeAsync(int id, int quantidade);
    }
}