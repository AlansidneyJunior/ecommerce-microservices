using Estoque.Domain.Exceptions;

namespace Estoque.Domain.Entities;

public class Produto
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal Preco { get; private set; }
    public int QuantidadeEstoque { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Ativo { get; private set; }

    private Produto() { }

    public Produto(string nome, string descricao, decimal preco, int quantidadeEstoque)
    {
        ValidarNome(nome);
        ValidarPreco(preco);
        ValidarQuantidade(quantidadeEstoque);

        Nome = nome;
        Descricao = descricao ?? string.Empty;
        Preco = preco;
        QuantidadeEstoque = quantidadeEstoque;
        DataCadastro = DateTime.UtcNow;
        Ativo = true;
    }

    // MÉTODOS DE NEGÓCIO

    public void AtualizarEstoque(int quantidade)
    {
        var novaQuantidade = QuantidadeEstoque + quantidade;

        if (novaQuantidade < 0)
            throw new DomainException($"Estoque insuficiente. Disponível: {QuantidadeEstoque}, Solicitado: {Math.Abs(quantidade)}");

        QuantidadeEstoque = novaQuantidade;
    }

    public void AtualizarPreco(decimal novoPreco)
    {
        ValidarPreco(novoPreco);
        Preco = novoPreco;
    }

    public void AtualizarNomeDescricao(string nome, string descricao)
    {
        ValidarNome(nome);

        Nome = nome;
        Descricao = descricao ?? string.Empty;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    public void Ativar()
    {
        Ativo = true;
    }

    // VALIDAÇÕES PRIVADAS (regras de negócio)

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do produto é obrigatório");

        if (nome.Length < 3)
            throw new DomainException("Nome do produto deve ter no mínimo 3 caracteres");

        if (nome.Length > 200)
            throw new DomainException("Nome do produto deve ter no máximo 200 caracteres");
    }

    private void ValidarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DomainException("Preço deve ser maior que zero");
    }

    private void ValidarQuantidade(int quantidade)
    {
        if (quantidade < 0)
            throw new DomainException("Quantidade inicial não pode ser negativa");
    }
}