using CartService;
using CartService.Profiles;
using CartService.Repositories;
using CartService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CartDbContext>(options => { options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!); });
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(MyMapper));
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
