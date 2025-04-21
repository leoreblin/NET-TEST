using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository.
    /// </summary>
    /// <param name="sale">The sale.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its identifier.
    /// </summary>
    /// <param name="id">The sale identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
