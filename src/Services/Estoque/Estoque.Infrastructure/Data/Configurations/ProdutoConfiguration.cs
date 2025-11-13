using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estoque.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estoque.Infrastructure.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Descricao)
                .HasMaxLength(1000);

            builder.Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.QuantidadeEstoque)
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired();

            builder.Property(p => p.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            // Índices para melhorar performance
            builder.HasIndex(p => p.Nome);
            builder.HasIndex(p => p.Ativo);
        }
    }
}