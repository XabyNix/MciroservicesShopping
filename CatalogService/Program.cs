using CatalogService.AsyncMessaging;
using CatalogService.Data;
using CatalogService.Grpc.Services;
using CatalogService.Handlers;
using CatalogService.Repositories;
using CatalogService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//dbContext
var connectionString = builder.Configuration.GetConnectionString("Aws");
builder.Services.AddDbContext<CatalogDbContext>(
    options => { options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*--------------------------------------------------------*/

builder.Services.AddControllers();
builder.Services.AddGrpc();

builder.Services.AddHostedService<Consumer>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<IReservedItemRepository, ReservedItemsRepository>();
builder.Services.AddScoped<IEventHandlers, EventHandlers>();
//Grpc external service
builder.Services.AddScoped<OrderService>();
/*--------------------------------------------------------*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<CatalogService.Services.CatalogService>();
//app.UseHttpsRedirection();
app.UseAuthorization();
PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

app.MapControllers();
app.Run();