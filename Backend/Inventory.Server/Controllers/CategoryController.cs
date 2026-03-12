using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public class CategoryController : EntityController<Category, SearchableCategory, CategoryController>
{
    /// <inheritdoc />
    public CategoryController(IEntityQueryService<Category, SearchableCategory> entityService, ILogger<CategoryController> logger) : base(entityService, logger)
    {
    }
}
