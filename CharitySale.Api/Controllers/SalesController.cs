using AutoMapper;
using CharitySale.Api.Services;
using CharitySale.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CharitySale.Api.Controllers;

/// <summary>
/// Controller for managing charity sales
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SalesController(ISaleService saleService,
    ILogger<SalesController> logger, IMapper mapper, IHubContext<CharitySaleHub> hubContext) : ControllerBase
{
    /// <summary>
    /// Creates a new sale record
    /// </summary>
    /// <param name="request">Sale creation request with items and quantities</param>
    /// <returns>The newly created sale</returns>
    /// <response code="201">Returns the newly created sale</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Sale))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Sale>> CreateSale([FromBody] CreateSale request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await saleService.CreateSaleAsync(request);
            if (!result.IsSuccess)
            {
                if (result.Error?.Contains("insufficient payment", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return UnprocessableEntity(result.Error);
                }
                if (result.Error?.Contains("insufficient stock", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return BadRequest(result.Error);
                }

                logger.LogError("Failed to create sale: {Error}", result.Error);
                return StatusCode(500, $"An error occurred while processing the sale. {result.Error}");
            }

            var sale = mapper.Map<Sale>(result.Value);
            var changeAmount = request.AmountPaid - sale.TotalAmount;
            
            sale.ChangeAmount = changeAmount;
            sale.Change = saleService.CalculateChangeDenominations(changeAmount);
            
            // Notify connected clients about the new sale
            await hubContext.Clients.All.SendAsync("SaleCreated", sale.Id);
            
            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating sale {@Request}", request);
            return StatusCode(500, "An unexpected error occurred while processing the sale.");
        }
    }

    /// <summary>
    /// Retrieves all sales
    /// </summary>
    /// <returns>A list of all sales</returns>
    /// <response code="200">Returns the list of sales</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Sale>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Sale>>> GetAllSales()
    {
        try
        {
            var result = await saleService.GetAllSalesAsync();
            if (!result.IsSuccess)
            {
                logger.LogError("Failed to retrieve sales: {Error}", result.Error);
                return StatusCode(500, "An error occurred while retrieving sales.");
            }

            var sales = mapper.Map<List<Sale>>(result.Value);
            return Ok(sales);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving sales");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieves a specific sale by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <returns>The requested sale</returns>
    /// <response code="200">Returns the requested sale</response>
    /// <response code="404">If the sale was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Sale))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Sale>> GetSale(Guid id)
    {
        try
        {
            var result = await saleService.GetSaleByIdAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return NotFound(result.Error);
                }

                logger.LogError("Failed to retrieve sale {Id}: {Error}", id, result.Error);
                return StatusCode(500, "An error occurred while retrieving the sale.");
            }

            var sale = mapper.Map<Sale>(result.Value);
            return Ok(sale);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving sale {Id}", id);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}