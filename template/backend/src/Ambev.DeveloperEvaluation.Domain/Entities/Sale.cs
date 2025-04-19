using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : AggregateRoot
{
    /// <summary>
    /// Gets the sale number.
    /// </summary>
    public string Number { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the sale occurred.
    /// </summary>
    public DateTime OccurredAt { get; private set; }

    /// <summary>
    /// Gets the customer identifier.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the customer associated with the sale.
    /// </summary>
    public User Customer { get; private set; } = default!;

    /// <summary>
    /// Gets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets the branch identifier where the sale occurred.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the branch associated with the sale.
    /// </summary>
    public Branch Branch { get; private set; } = default!;

    /// <summary>
    /// Gets a value indicating whether the sale is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    private readonly List<SaleItem> _items = [];
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    private Sale() { }

    public Sale(
        Guid id,
        string number,
        DateTime occuredAt,
        Guid customerId,
        Guid branchId) : base(id)
    {
        Number = number;
        OccurredAt = occuredAt;
        CustomerId = customerId;
        BranchId = branchId;
        IsCancelled = false;

        Raise(new SaleCreatedEvent(this));
    }

    /// <summary>
    /// Adds an item to the sale.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    /// <exception cref="DomainException"></exception>
    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (IsCancelled)
        {
            throw new DomainException("Cannot modify cancelled sale.");
        }

        if (quantity > 20)
        {
            throw new DomainException("Maximum 20 items per product allowed.");
        }

        var discount = CalculateDiscount(quantity);
        var itemTotal = CalculateItemTotal(quantity, unitPrice, discount);

        _items.Add(new SaleItem(
            productId,
            quantity,
            unitPrice,
            discount,
            itemTotal
        ));

        UpdateTotalAmount();
        Raise(new SaleModifiedEvent(this));
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled) return;

        IsCancelled = true;
        Raise(new SaleCancelledEvent(this));
    }

    /// <summary>
    /// Updates the total amount of the sale.
    /// </summary>
    public void UpdateTotalAmount() =>
        TotalAmount = _items.Sum(item => item.Total);

    /// <summary>
    /// Calculates the discount based on the quantity of items.
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    private static decimal CalculateDiscount(int quantity) => 
        quantity switch
        {
            >= 10 and <= 20 => 0.20m,
            >= 4 and < 10 => 0.10m,
            _ => 0m
        };

    /// <summary>
    /// Calculates the total price of an item based on quantity, unit price, and discount.
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    /// <param name="discount"></param>
    /// <returns></returns>
    private static decimal CalculateItemTotal(int quantity, decimal unitPrice, decimal discount) =>
        quantity * unitPrice * (1 - discount);
}
