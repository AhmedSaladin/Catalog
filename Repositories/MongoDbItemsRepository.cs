using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
  public class MongoDbItemsRepository : IItemsRepository
  {
    private const string databaseName = "catalog";

    private const string collectionName = "items";

    private readonly IMongoCollection<Item> itemsCollection;

    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

    public MongoDbItemsRepository(IMongoClient mongoClient)
    {
      IMongoDatabase database = mongoClient.GetDatabase(databaseName);
      itemsCollection = database.GetCollection<Item>(collectionName);
    }

    public void CreateItemAsync(Item item)
    {
      itemsCollection.InsertOne(item);
    }

    public void DeleteItemAsync(Guid id)
    {
      var filter = Filter(id);
      itemsCollection.FindOneAndDelete(filter);
    }

    public Item GetItemAsync(Guid id)
    {
      var filter = Filter(id);
      return itemsCollection.Find(filter).SingleOrDefault();
    }

    IEnumerable<Item> IItemsRepository.GetItemsAsync()
    {
      return itemsCollection.Find(new BsonDocument()).ToList();
    }

    public void UpdateItemAsync(Item item)
    {
      var filter = Filter(item.Id);
      itemsCollection.ReplaceOne(filter, item);
    }

    private FilterDefinition<Item> Filter(Guid id)
    {
      return filterBuilder.Eq(item => item.Id, id);
    }
  }
}