using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using WebApplication1.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Register MongoDB and configure database/collection creation 
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    
    BsonSerializer.RegisterSerializer(typeof(decimal), new MongoDB.Bson.Serialization.Serializers.DecimalSerializer(BsonType.Decimal128));

    
    var client = new MongoClient("mongodb://localhost:27017");

    
    var databaseName = "MyNewDatabase"; 
    var database = client.GetDatabase(databaseName);

    
    var collectionName = "MyNewCollection"; 

    
    var collectionList = database.ListCollectionNamesAsync().Result.ToList();
    if (!collectionList.Contains(collectionName))
    {
        database.CreateCollection(collectionName);
        //Console.WriteLine($"Collection '{collectionName}' created successfully in database '{databaseName}'.");
    }
    //else
    //{
    //    Console.WriteLine($"Collection '{collectionName}' already exists in database '{databaseName}'.");
    //}

    return database;
});

builder.Services.AddScoped<EmpRegisterController>(sp =>
{
    return new EmpRegisterController(sp.GetRequiredService<IMongoDatabase>());
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
