using AutoMapper;
using CharitySale.Api.Services;
using CharitySale.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CharitySale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SalesController(ISaleService saleService, ILogger<SalesController> logger, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
            
            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating sale {@Request}", request);
            return StatusCode(500, "An unexpected error occurred while processing the sale.");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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