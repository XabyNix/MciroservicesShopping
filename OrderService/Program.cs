using Microsoft.EntityFrameworkCore;
using OrderService.AsyncComunication;
using OrderService.Data;
using OrderService.Grpc;
using OrderService.Handlers;
using OrderService.Repository;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//dbContext
var connectionString = builder.Configuration.GetConnectionString("Aws");
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderCreator, OrderCreator>();
builder.Services.AddScoped<IEventHandlers, EventHandlers>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddHostedService<Consumer>();
builder.Services.AddGrpc();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());

app.MapGrpcService<GrpcOrderService>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();