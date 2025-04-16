namespace Ambev.DeveloperEvaluation.Domain.Abstractions;

public interface IEventHandler<in TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Handles the specified event asynchronously.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The completed task.</returns>
    Task Handle(TEvent @event, CancellationToken cancellationToken = default);
}

public interface IEventHandler
{
    /// <summary>
    /// Handles the specified event asynchronously.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task Handle(IEvent @event, CancellationToken cancellationToken = default);
}
