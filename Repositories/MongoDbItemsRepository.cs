using Catalog.Entities;
using MongoDB.Driver;

namespace Catalog.Repositories
{
  public class MongoDbItemsRepository : IItemsRepository
  {
    private const string databaseName = "catalog";

    private const string collectionName = "items";

    private readonly IMongoCollection<Item> itemsCollection;

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
      throw new NotImplementedException();
    }

    Item IItemsRepository.GetItem(Guid id)
    {
      throw new NotImplementedException();
    }

    IEnumerable<Item> IItemsRepository.GetItems()
    {
      throw new NotImplementedException();
    }

    void IItemsRepository.UpdateItem(Item item)
    {
      throw new NotImplementedException();
    }
  }
}