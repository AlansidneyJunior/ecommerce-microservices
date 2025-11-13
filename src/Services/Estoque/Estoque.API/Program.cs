using Estoque.Application.Interfaces;
using Estoque.Application.Mappings;
using Estoque.Application.Services;
using Estoque.Application.Validators;
using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Data;
using Estoque.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ========== CONFIGURAÇÃO DE SERVIÇOS ==========

// 1. BANCO DE DADOS (Entity Framework + PostgreSQL)
builder.Services.AddDbContext<EstoqueDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null
        )
    )
);

// 2. REPOSITORIES (Acesso a dados)
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// 3. SERVICES (Lógica de aplicação)
builder.Services.AddScoped<IProdutoService, ProdutoService>();

// 4. AUTOMAPPER (Mapeamento de DTOs)
builder.Services.AddAutoMapper(typeof(ProdutoMappingProfile));

// 5. FLUENTVALIDATION (Validação de DTOs)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CriarProdutoDtoValidator>();

// 6. CONTROLLERS
builder.Services.AddControllers();

// 7. SWAGGER (Documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 8. CORS (Permitir acesso de outros domínios)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 9. LOGGING
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// 10. HEALTH CHECKS (Verificar saúde da aplicação)
builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgres",
        tags: new[] { "db", "postgres" }
    );

var app = builder.Build();

// ========== APLICAR MIGRATIONS AUTOMATICAMENTE ==========
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var dbContext = services.GetRequiredService<EstoqueDbContext>();
        
        logger.LogInformation("🔄 Verificando conexão com o banco de dados PostgreSQL...");
        
        // Verifica se o banco está acessível
        if (dbContext.Database.CanConnect())
        {
            logger.LogInformation("✅ Conexão com PostgreSQL estabelecida!");
            
            // Aplica migrations pendentes
            logger.LogInformation("🔄 Aplicando migrations...");
            dbContext.Database.Migrate();
            logger.LogInformation("✅ Migrations aplicadas com sucesso!");
        }
        else
        {
            logger.LogError("❌ Não foi possível conectar ao PostgreSQL. Verifique a connection string.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Erro ao inicializar o banco de dados: {Message}", ex.Message);
        // Em produção, você pode querer não iniciar a aplicação se o banco falhar
        // throw;
    }
}

// ========== PIPELINE DE REQUISIÇÕES ==========

// Swagger (sempre habilitado para facilitar testes)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estoque API V1");
    c.RoutePrefix = string.Empty; // Swagger na raiz (http://localhost:5001)
    c.DocumentTitle = "Estoque API - Documentação";
});

// CORS
app.UseCors("AllowAll");

// HTTPS Redirect (comentado para desenvolvimento local)
// app.UseHttpsRedirection();

// Autenticação e Autorização (será adicionado depois)
app.UseAuthentication();
app.UseAuthorization();

// Health Checks
app.MapHealthChecks("/health");

// Controllers
app.MapControllers();

// ========== MENSAGENS DE INICIALIZAÇÃO ==========
var port = app.Urls.FirstOrDefault() ?? "http://localhost:5001";
var environment = app.Environment.EnvironmentName;

Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
Console.WriteLine("║           🚀 ESTOQUE API - MICROSERVIÇO                  ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
Console.WriteLine($"📍 Ambiente: {environment}");
Console.WriteLine($"🌐 URL: {port}");
Console.WriteLine($"📚 Swagger: {port}");
Console.WriteLine($"💚 Health Check: {port}/health");
Console.WriteLine($"🗄️  Banco: PostgreSQL");
Console.WriteLine("╚══════════════════════════════════════════════════════════╝");

app.Run();