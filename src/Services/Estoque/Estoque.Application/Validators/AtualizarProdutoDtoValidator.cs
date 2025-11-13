using Estoque.Application.DTOs;
using FluentValidation;

namespace Estoque.Application.Validators
{
    public class AtualizarProdutoDtoValidator : AbstractValidator<AtualizarProdutoDto>
    {
        public AtualizarProdutoDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres");

            RuleFor(x => x.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero");

            RuleFor(x => x.QuantidadeEstoque)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque não pode ser negativa");
        }
    }
}
