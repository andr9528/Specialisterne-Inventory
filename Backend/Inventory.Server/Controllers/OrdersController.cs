using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public sealed class OrdersController : EntityController<Order, SearchableOrder, OrdersController>
{
    public OrdersController(
        IEntityQueryService<Order, SearchableOrder> entityService,
        ILogger<OrdersController> logger) : base(entityService, logger)
    {
    }

    // Todo: Implement during Pair-Programming
    [HttpPost]
    public Task<IActionResult> CreateOrder(/*Add Any needed arguments*/)
    {
        throw new NotImplementedException();
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
