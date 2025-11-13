using AutoMapper;
using Estoque.Application.DTOs;
using Estoque.Application.Interfaces;
using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;
using Estoque.Domain.Exceptions;

namespace Estoque.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProdutoDto>> ObterTodosAsync()
        {
            var produtos = await _repository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }

        public async Task<ProdutoDto> ObterPorIdAsync(int id)
        {
            var produto = await _repository.ObterPorIdAsync(id);

            if (produto == null)
                throw new NotFoundException($"Produto com ID {id} não encontrado");

            return _mapper.Map<ProdutoDto>(produto);
        }

        public async Task<ProdutoDto> CriarAsync(CriarProdutoDto dto)
        {
            var produto = new Produto(
                dto.Nome,
                dto.Descricao,
                dto.Preco,
                dto.QuantidadeEstoque
            );

            var produtoCriado = await _repository.AdicionarAsync(produto);

            return _mapper.Map<ProdutoDto>(produtoCriado);
        }

        public async Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto dto)
        {
            var produto = await _repository.ObterPorIdAsync(id);

            if (produto == null)
                throw new NotFoundException($"Produto com ID {id} não encontrado");

            produto.AtualizarNomeDescricao(dto.Nome, dto.Descricao);
            produto.AtualizarPreco(dto.Preco);
            produto.AtualizarEstoque(dto.QuantidadeEstoque - produto.QuantidadeEstoque);

            await _repository.AtualizarAsync(produto);

            return _mapper.Map<ProdutoDto>(produto);
        }

        public async Task DeletarAsync(int id)
        {
            var produto = await _repository.ObterPorIdAsync(id);

            if (produto == null)
                throw new NotFoundException($"Produto com ID {id} não encontrado");

            // Inativa o produto (soft delete)
            produto.Desativar();

            await _repository.AtualizarAsync(produto);
        }

        public async Task AtualizarEstoqueAsync(int id, AtualizarEstoqueDto dto)
        {
            var produto = await _repository.ObterPorIdAsync(id);

            if (produto == null)
                throw new NotFoundException($"Produto com ID {id} não encontrado");

            // Usa método do Domain que valida estoque
            produto.AtualizarEstoque(dto.Quantidade);

            await _repository.AtualizarAsync(produto);
        }

        public async Task<bool> VerificarDisponibilidadeAsync(int id, int quantidade)
        {
            var produto = await _repository.ObterPorIdAsync(id);

            if (produto == null)
                return false;

            return produto.QuantidadeEstoque >= quantidade;
        }
    }
}