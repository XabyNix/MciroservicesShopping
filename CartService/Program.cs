using CartService;
using CartService.AsyncMessage;
using CartService.Data;
using CartService.Repositories;
using CartService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//dbContext
var connectionString = builder.Configuration.GetConnectionString("SQLServer");
builder.Services.AddDbContext<CartDbContext>(options => { options.UseSqlServer(connectionString); });

// Add services to the container.
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();