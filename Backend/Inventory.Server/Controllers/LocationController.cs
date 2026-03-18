using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public class LocationController : EntityController<Location, SearchableLocation, LocationController, IComplexSearchable<SearchableLocation>>
{
    /// <inheritdoc />
    public LocationController(IEntityQueryService<Location, SearchableLocation> entityService, ILogger<LocationController> logger) : base(entityService, logger)
    {
    }
}
