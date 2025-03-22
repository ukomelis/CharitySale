using AutoMapper;
using CharitySale.Api.Services;
using CharitySale.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CharitySale.Api.Controllers;

/// <summary>
/// Controller for managing charity sale items
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ItemsController(IItemService itemService,
    ILogger<ItemsController> logger, IMapper mapper, IHubContext<CharitySaleHub> hubContext) : ControllerBase
{
    /// <summary>
    /// Retrieves all items available for charity sale
    /// </summary>
    /// <returns>A collection of all items</returns>
    /// <response code="200">Returns the list of items</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Item>))]
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

    /// <summary>
    /// Retrieves a specific item by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the item</param>
    /// <returns>The requested item</returns>
    /// <response code="200">Returns the requested item</response>
    /// <response code="404">If the item was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
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
    
    /// <summary>
    /// Sets stock of a specific item
    /// </summary>
    /// <param name="id">The unique identifier of the item</param>
    /// <param name="request">The request containing the new quantity</param>
    /// <returns>The updated item</returns>
    /// <response code="200">Returns the updated item</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the item was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPatch("{id:guid}/quantity")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Item>> SetItemStock(Guid id, [FromBody] UpdateItemQuantity request)
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
            
            // Notify all connected clients about the quantity update
            await hubContext.Clients.All.SendAsync("ItemQuantityUpdated", id, request.Quantity);

            return Ok(updatedItem);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating item quantity {Id}", id);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}