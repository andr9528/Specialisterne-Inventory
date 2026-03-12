using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public class ProductController : EntityController<Product, SearchableProduct, ProductController>
{
    /// <inheritdoc />
    public ProductController(IEntityQueryService<Product, SearchableProduct> entityService, ILogger<ProductController> logger) : base(entityService, logger)
    {
    }
}
