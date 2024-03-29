using AuthenticationService.Data;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

//Add Authentication
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AuthDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//app.MapIdentityApi<User>();
app.MapControllers();

app.Run();