using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Assignment7.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Assignment7Context>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("Assignment7Context") ?? throw new InvalidOperationException("Connection string 'Assignment7Context' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
