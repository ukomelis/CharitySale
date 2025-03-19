using AutoMapper;
using CharitySale.Api.Services;
using CharitySale.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CharitySale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ItemsController(IItemService itemService, ILogger<ItemsController> logger, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
    {
        try
        {
            var result = await itemService.GetAllItemsAsync();
            if (!result.IsSuccess)
            {
                logger.LogError("Failed to retrieve items: {Error}", result.Error);
                return StatusCode(500, "An error occurred while retrieving items.");
            }

            var items = mapper.Map<IEnumerable<Item>>(result.Value);
            return Ok(items);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving items");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Item>> GetItem(Guid id)
    {
        try
        {
            var result = await itemService.GetItemByIdAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return NotFound(result.Error);
                }
                
                logger.LogError("Failed to retrieve item {Id}: {Error}", id, result.Error);
                return StatusCode(500, "An error occurred while retrieving the item.");
            }

            var item = mapper.Map<Item>(result.Value);
            return Ok(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving item {Id}", id);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpPatch("{id:guid}/quantity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Item>> UpdateItemQuantity(Guid id, [FromBody] UpdateItemQuantity request)
    {
        try
        {
            var result = await itemService.SetItemStockAsync(id, request.Quantity);
            if (!result.IsSuccess)
            {
                if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return NotFound(result.Error);
                }

                logger.LogError("Failed to update item quantity {Id}: {Error}", id, result.Error);
                return StatusCode(500, "An error occurred while updating the item quantity.");
            }

            var updatedItem = mapper.Map<Item>(result.Value);
            return Ok(updatedItem);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating item quantity {Id}", id);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}