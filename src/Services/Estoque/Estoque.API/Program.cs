using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Data;
using Estoque.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar Controllers
builder.Services.AddControllers();

// Configurar DbContext com PostgreSQL
builder.Services.AddDbContext<EstoqueDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositórios
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();