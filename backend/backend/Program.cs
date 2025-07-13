using backend.Application;
using backend.Database;
using backend.Infraestructure;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
      policy.WithOrigins("http://localhost:8080");
      policy.AllowAnyMethod();
      policy.AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductsQuery, ProductsQuery>();

builder.Services.AddScoped<ICoinsQuery, CoinsQuery>();

builder.Services.AddScoped<IPaymentOrchestrator, PaymentOrchestrator>();

builder.Services.AddScoped<IProductPaymentCommand, ProductPaymentCommand>();

builder.Services.AddScoped<ICoinPaymentCommand, CoinPaymentCommand>();

builder.Services.AddScoped<ICoinsRepository, CoinsRepository>();

builder.Services.AddScoped<ICoinPaymentRepository, CoinPaymentRepository>();

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

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

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
