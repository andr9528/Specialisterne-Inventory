using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Dto.Create;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Server.Controllers.Core;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers;

[Route(Constants.ROUTE_TEMPLATE)]
[ApiController]
public class ProductController : EntityController<Product, SearchableProduct, ProductController, ComplexSearchableProduct>
{
    private readonly IEntityQueryService<Category, SearchableCategory> categoryService;

    /// <inheritdoc />
    public ProductController(
        IEntityQueryService<Product, SearchableProduct> entityService, ILogger<ProductController> logger,
        IEntityQueryService<Category, SearchableCategory> categoryService) : base(entityService, logger)
    {
        this.categoryService = categoryService;
    }

    [HttpPost]
    public override Task<IActionResult> AddSingle(Product entity)
    {
        return Task.FromResult<IActionResult>(MethodNotSupported(nameof(AddSingle)));
    }

    [HttpPost]
    public override Task<IActionResult> AddMultiple([FromBody] IEnumerable<Product> entities)
    {
        return Task.FromResult<IActionResult>(MethodNotSupported(nameof(AddMultiple)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        try
        {
            IProduct product = await dto.UnpackDto(categoryService);
            await entityService.AddEntity((Product) product);

            return Ok(product);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Caught exception during {MethodName}.", nameof(CreateProduct));
            throw;
        }
    }
}
