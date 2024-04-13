using OrderService.AsyncComunication;
using OrderService.Grpc;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//dbContext
//builder.Services.AddDbContext<OrderDbContext>(options => { options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("DefaultConnection")!); });

builder.Services.AddControllers();
builder.Services.AddScoped<CatalogService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderProcess, OrderProcess>();
builder.Services.AddHostedService<MessageService>();

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
