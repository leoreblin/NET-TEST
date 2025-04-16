using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Abstractions;

/// <summary>
/// Represents the unit of work concrete class.
/// </summary>
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly DefaultContext _context;
    private readonly ILogger<UnitOfWork> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The DB context.</param>
    /// <param name="logger">The logger.</param>
    public UnitOfWork(DefaultContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes to the database.");
            transaction.Rollback();
            throw;
        }
    }
}
