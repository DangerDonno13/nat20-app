using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nat20Server.Database;
using Nat20Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("MongoDb"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp
        .GetRequiredService<IOptions<DatabaseSettings>>()
        .Value;

    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();

    var settings = sp
        .GetRequiredService<IOptions<DatabaseSettings>>()
        .Value;

    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();