using AutoMapper;
using Estoque.Application.DTOs;
using Estoque.Domain.Entities;

namespace Estoque.Application.Mappings
{
    public class ProdutoMappingProfile : Profile
    {
        public ProdutoMappingProfile()
        {
            CreateMap<Produto, ProdutoDto>();

            CreateMap<CriarProdutoDto, Produto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID é gerado pelo banco
                .ForMember(dest => dest.Ativo, opt => opt.Ignore()) // Definido no construtor
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore()); // Definido no construtor
        }
    }
}