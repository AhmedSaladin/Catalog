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

    void IItemsRepository.CreateItem(Item item)
    {
      itemsCollection.InsertOne(item);
    }

    void IItemsRepository.DeleteItem(Guid id)
    {
      var filter = Filter(id);
      itemsCollection.FindOneAndDelete(filter);
    }

    Item IItemsRepository.GetItem(Guid id)
    {
      var filter = Filter(id);
      return itemsCollection.Find(filter).SingleOrDefault();
    }

    IEnumerable<Item> IItemsRepository.GetItems()
    {
      return itemsCollection.Find(new BsonDocument()).ToList();
    }

    void IItemsRepository.UpdateItem(Item item)
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