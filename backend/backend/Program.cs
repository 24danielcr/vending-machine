using backend.Application;
using backend.Database;
using backend.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICoinsQuery, CoinsQuery>();

builder.Services.AddScoped<IPaymentOrchestrator, PaymentOrchestrator>();

builder.Services.AddScoped<IProductPaymentCommand, ProductPaymentCommand>();

builder.Services.AddScoped<ICoinPaymentCommand, CoinPaymentCommand>();

builder.Services.AddScoped<ICoinsRepository, CoinsRepository>();

builder.Services.AddScoped<ICoinPaymentRepository, CoinPaymentRepository>();

builder.Services.AddScoped<IPaymentOrchestratorRepository
  , PaymentOrchestratorRepository>();

builder.Services.AddScoped<IProductPaymentRepository
  , ProductPaymentRepository>();

builder.Services.AddScoped<IProductPaymentRepository
  , ProductPaymentRepository>();

builder.Services.AddSingleton<VendingMachineDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
