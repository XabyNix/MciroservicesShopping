using CatalogService;
using CatalogService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

//builder.Services.AddHostedService<MessageService>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();


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
