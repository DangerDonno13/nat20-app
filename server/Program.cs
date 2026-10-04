using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Retrieves the connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("MongoDB");

var app = builder.Build();

app.MapGet("/test-db", async () =>
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        return Results.Problem("MongoDB connection string is missing from configuration.");
    }

    try
    {
        // Initialize the MongoDB client
        var client = new MongoClient(connectionString);

        // Send a ping command to the 'admin' database to verify connectivity
        var database = client.GetDatabase("admin");
        var pingResult = await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));

        return Results.Ok(new
        {
            Message = "Successfully connected to MongoDB Atlas!",
            PingResponse = pingResult.ToString()
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to connect to MongoDB: {ex.Message}");
    }
});

app.Run();