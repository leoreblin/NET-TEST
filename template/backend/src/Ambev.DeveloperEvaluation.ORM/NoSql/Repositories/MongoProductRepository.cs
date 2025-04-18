using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.NoSql.Context;
using Ambev.DeveloperEvaluation.ORM.NoSql.Models;
using AutoMapper;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.NoSql.Repositories;

public class MongoProductRepository : IProductRepository
{
    private readonly IMongoCollection<ProductDocument> _products;
    private readonly IMapper _mapper;

    public MongoProductRepository(MongoDbContext context, IMapper mapper)
    {
        _products = context.Products;
        _mapper = mapper;
    }

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
}
