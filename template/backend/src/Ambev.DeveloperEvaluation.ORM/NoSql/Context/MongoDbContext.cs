using Ambev.DeveloperEvaluation.ORM.NoSql.Configurations;
using Ambev.DeveloperEvaluation.ORM.NoSql.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.NoSql.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public MongoDbContext(
        IOptions<MongoDbSettings> settings,
        IMongoClient client)
    {
        _settings = settings.Value;
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<ProductDocument> Products =>
        _database.GetCollection<ProductDocument>(_settings.ProductsCollection);
}
