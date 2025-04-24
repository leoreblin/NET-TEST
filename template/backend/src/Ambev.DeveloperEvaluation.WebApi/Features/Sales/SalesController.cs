using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("sales")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;

    public SalesController(
        IMediator mediator, 
        IMapper mapper, 
        ISaleRepository saleRepository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="request">The sale creation request.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created sale details.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var requestValidator = new CreateSaleRequestValidator();
        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<Guid>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = response.Id
        });
    }

    /// <summary>
    /// Creates a new sale from the customer's cart.
    /// </summary>
    /// <param name="request">The sale creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created sale details.</returns>
    [HttpPost("from-cart")]
    [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFromCart(
        [FromBody] CreateSaleFromCartRequest request, 
        CancellationToken cancellationToken)
    {
        var requestValidator = new CreateSaleFromCartRequestValidator();
        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<Guid>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = response.Id
        });
    }

    /// <summary>
    /// Cancels a sale.
    /// </summary>
    /// <param name="id">The sale identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// 1) <see cref="NoContentResult"/> if it's cancelled. <br></br>
    /// 2) <see cref="NotFoundResult"/> if the sale was not found. <br></br>
    /// 3) <see cref="BadRequestResult"/> if the identifier was invalid.</returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid sale identifier.");
        }

        var sale = await _saleRepository.GetByIdAsync(id, cancellationToken);
        if (sale is null)
        {
            return NotFound();
        }

        // todo: cancel the sale

        return NoContent();
    }
}
