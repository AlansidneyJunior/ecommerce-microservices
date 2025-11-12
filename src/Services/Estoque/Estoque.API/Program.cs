var builder = WebApplication.CreateBuilder(args);

// Adicionar Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();