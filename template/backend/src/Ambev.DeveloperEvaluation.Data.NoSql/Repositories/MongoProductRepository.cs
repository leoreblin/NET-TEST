using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Data.NoSql.Context;
using Ambev.DeveloperEvaluation.Data.NoSql.Models;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.Data.NoSql.Repositories;

public class MongoProductRepository : IProductRepository
{
    private readonly IMongoCollection<ProductDocument> _products;
    private readonly IMapper _mapper;

    public MongoProductRepository(MongoDbContext context, IMapper mapper)
    {
        _products = context.Products;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var products = await _products.FindAsync(
            Builders<ProductDocument>.Filter.Eq(p => p.ExternalId, productId),
            cancellationToken: cancellationToken);

        var product = await products.FirstOrDefaultAsync(cancellationToken);

        return product is null ? null : _mapper.Map<Product>(product);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Product>> GetByIdsAsync(
        IEnumerable<Guid> productIds, 
        CancellationToken cancellationToken = default)
    {
        var distinctIds = productIds.Distinct().ToList();
        var filter = Builders<ProductDocument>.Filter.In(p => p.ExternalId, distinctIds);

        var products = await _products.FindAsync(filter, cancellationToken: cancellationToken);
        var productDocuments = await products.ToListAsync(cancellationToken);

        var productEntities = productDocuments.Select(_mapper.Map<Product>).ToList();
        return productEntities;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<Product>> GetPaginatedAsync(
        int pageNumber, 
        int pageSize,
        string? orderBy = null,
        bool isDescending = false,
        string? term = null)
    {
        var filter = Builders<ProductDocument>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(term))
        {
            filter = Builders<ProductDocument>.Filter.Text(term);
        }

        var found = _products.Find(filter);
    
        var paginatedResult = await found.ToPagedListAsync(pageNumber, pageSize, orderBy, isDescending);

        return paginatedResult.Map(_mapper.Map<Product>);
    }

    /// <inheritdoc />
    public async Task<bool> ProductsExistAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken = default)
    {
        var distinctIds = productIds.Distinct().ToList();
        var filter = Builders<ProductDocument>.Filter.In(p => p.ExternalId, distinctIds);
        var existingCount = await _products.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return existingCount == distinctIds.Count;
    }
}
