using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale item.
/// </summary>
public sealed class SaleItem : BaseEntity
{
    /// <summary>
    /// Represents the product identifier.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the quantity of the product.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount applied to the product.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total price of the product after applying the discount.
    /// </summary>
    public decimal Total { get; private set; }

    /// <summary>
    /// Constructor required for EF Core.
    /// </summary>
    private SaleItem() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class with the specified parameters.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantity">The quantity of products.</param>
    /// <param name="unitPrice">The product unit price.</param>
    /// <param name="discount">The discount applied.</param>
    /// <param name="total">The total price after applying the discount.</param>
    public SaleItem(
        Guid productId,
        int quantity,
        decimal unitPrice,
        decimal discount,
        decimal total)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Total = total;
    }
}
