using Catalog.Repositories;
using Catalog.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSettings = builder.Configuration.GetSection("MongoDatabaseSettings").Get<MongoDbSettings>();


// Add services to the container.
builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
{
  BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
  BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
  return new MongoClient(mongoDbSettings.connection);
});

builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();

builder.Services.AddControllers(options =>
{
  options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks().AddMongoDb(mongoDbSettings.connection, name: "mongodb", timeout: TimeSpan.FromSeconds(3));

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

app.MapHealthChecks("/health");

app.Run();
