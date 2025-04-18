using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : AggregateRoot
{
    public string Number { get; private set; }

    public DateTime OccurredAt { get; private set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; private set; }


}
