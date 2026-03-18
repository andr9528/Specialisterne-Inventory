using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public class LocationItemController : EntityController<LocationItem, SearchableLocationItem, LocationItemController, ComplexSearchableLocationItem>
{
    /// <inheritdoc />
    public LocationItemController(IEntityQueryService<LocationItem, SearchableLocationItem> entityService, ILogger<LocationItemController> logger) : base(entityService, logger)
    {
    }
}
