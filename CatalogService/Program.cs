using CatalogService;
using CatalogService.AsyncMessaging;
using CatalogService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddHostedService<MessageService>();
builder.Services.AddGrpc();

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
//PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

app.MapControllers();
app.Run();
