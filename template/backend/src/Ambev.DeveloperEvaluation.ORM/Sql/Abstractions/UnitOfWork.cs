using Ambev.DeveloperEvaluation.Application.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Sql.Abstractions;

/// <summary>
/// Represents the unit of work concrete class.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DefaultContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private readonly IPublisher _publisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The DB context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="publisher">The MediatR publisher.</param>
    public UnitOfWork(DefaultContext context, ILogger<UnitOfWork> logger, IPublisher publisher)
    {
        _context = context;
        _logger = logger;
        _publisher = publisher;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var domainEvents = _context.ChangeTracker.Entries<AggregateRoot>()
                .Select(agg => agg.Entity)
                .Where(agg => agg.Events.Count != 0)
                .SelectMany(agg => agg.Events);

            await _context.SaveChangesAsync(cancellationToken);
            transaction.Commit();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes to the database.");
            transaction.Rollback();
            throw;
        }
    }
}
