using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Abstraction.Interfaces.Services;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public sealed class OrdersController : EntityController<Order, SearchableOrder, OrdersController>
{
    private readonly IOrderService orderService;

    public OrdersController(
        IEntityQueryService<Order, SearchableOrder> entityService,
        ILogger<OrdersController> logger, IOrderService orderService) : base(entityService, logger)
    {
        this.orderService = orderService;
    }

    [HttpPost("reference")]
    public async Task<IActionResult> CreateOrder(Guid reference, [FromBody] IEnumerable<OrderItem> items, [FromQuery] int? locationId)
    {
        try
        {
            IList<IOrder> orders = await orderService.CreateOrders(reference, items, locationId);
            await entityService.AddEntities(orders.Cast<Order>());

            return Ok(orders);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Caught exception during {MethodName}.", nameof(CreateOrder));
            throw;
        }
    }


    [HttpPost]
    public override Task<IActionResult> AddSingle([FromBody] Order entity)
    {
        return Task.FromResult<IActionResult>(MethodNotSupported(nameof(AddSingle)));
    }

    [HttpPost]
    public override Task<IActionResult> AddMultiple([FromBody] IEnumerable<Order> entities)
    {
        return Task.FromResult<IActionResult>(MethodNotSupported(nameof(AddMultiple)));
    }
}
