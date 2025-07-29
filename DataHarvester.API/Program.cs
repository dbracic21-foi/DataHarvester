using DataHarvester.API.Services;
using DataHarvester.Infrastructure.Persistence;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using DataHarvester.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton<QueueSenderServices>();
builder.Services.AddControllers();
builder.Services.AddScoped<IDataItemRepository, DataItemRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
