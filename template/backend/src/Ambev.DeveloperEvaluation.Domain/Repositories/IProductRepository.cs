using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    /// <summary>
    /// Get paginated products.
    /// </summary>
    /// <param name="pageNumber">The number of the page.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="orderBy">The property to sort by.</param>
    /// <param name="isDescending">The sort direction.</param>
    /// <returns></returns>
    Task<PaginatedList<Product>> GetPaginatedAsync(
        int pageNumber, 
        int pageSize,
        string? orderBy = null,
        bool isDescending = false,
        string? term = null);
}
