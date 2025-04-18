namespace Ambev.DeveloperEvaluation.ORM.NoSql.Configurations;

/// <summary>
/// Represents the MongoDB settings.
/// </summary>
public class MongoDbSettings
{
    public const string ConfigurationSection = "MongoDb";

    /// <summary>
    /// Gets or sets the connection string to the MongoDB server.
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    public string DatabaseName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the products collection.
    /// </summary>
    public string ProductsCollection { get; init; } = "products";

    /// <summary>
    /// Gets or sets the name of the customers collection.
    /// </summary>
    public string CustomersCollection { get; init; } = "customers";
}
