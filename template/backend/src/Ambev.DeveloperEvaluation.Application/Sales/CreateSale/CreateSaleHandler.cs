using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

internal sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly IUserRepository _userRepository;

    public CreateSaleHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var customer = await _userRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new ValidationException($"There's no customer of ID {request.CustomerId}.");

        var sale = new Sale(
            Guid.NewGuid(),
            GenerateSaleNumber(),
            DateTime.UtcNow,
            customer.Id,
            request.BranchId);

        throw new NotImplementedException();
    }

    private static string GenerateSaleNumber()
        => $"SALE-{DateTime.UtcNow:yyyyMMdd-HHmmss0fff}";
}
