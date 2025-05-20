using System.Reflection;
using Contato.Delete.Application.Interfaces;
using Contato.Delete.Application.Services;
using Contato.Delete.Infra.RabbitMQ;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Ambiente atual: {builder.Environment.EnvironmentName}");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IAsyncRabbitMqProducer, RabbitMqProducer>();

builder.Services.AddScoped<IContatoService, ContatoService>();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Define a porta HTTP
builder.WebHost.UseUrls("http://*:8080");

var app = builder.Build();

// (Opcional) Habilitar CORS para liberar requisições de qualquer origem
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();

// Middleware Prometheus para requisições HTTP — deve vir antes da autorização
app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllers();

// Mapear /metrics depois dos controllers para evitar conflitos de rota
app.MapMetrics();

app.Run();
