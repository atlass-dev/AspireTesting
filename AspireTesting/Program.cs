using AspireTesting;
using AspireTesting.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddServiceDefaults();

// Add services to the container.

var databaseConnectionString = configuration.GetConnectionString("AppDatabase");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
            databaseConnectionString,
            sqlOptions => sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name)
    );
});
builder.Services.AddAsyncInitializer<DatabaseInitializer>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

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
