using CartService;
using CartService.Profiles;
using CartService.Repositories;
using CartService.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

//dbContext
builder.Services.AddDbContext<CartDbContext>(options => { options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!); });

// Add services to the container.
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();



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

PrepDb.PrepPopulation(app);

app.Run();
