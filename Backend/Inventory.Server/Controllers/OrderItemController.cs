using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public class OrderItemController : EntityController<OrderItem, SearchableOrderItem, OrderItemController, IComplexSearchable<SearchableOrderItem>>
{
    /// <inheritdoc />
    public OrderItemController(IEntityQueryService<OrderItem, SearchableOrderItem> entityService, ILogger<OrderItemController> logger) : base(entityService, logger)
    {
    }
}
