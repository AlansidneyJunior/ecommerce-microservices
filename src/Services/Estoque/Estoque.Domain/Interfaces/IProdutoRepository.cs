using Estoque.Domain.Entities;

namespace Estoque.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> ObterPorIdAsync(int id);
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<IEnumerable<Produto>> ObterAtivosAsync();
        Task<Produto> AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task<bool> ExisteAsync(int id);
    }
}