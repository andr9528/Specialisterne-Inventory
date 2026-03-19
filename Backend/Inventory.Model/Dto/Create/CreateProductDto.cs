using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;

namespace Inventory.Model.Dto.Create;

public class CreateProductDto
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public IEnumerable<CreateLocationItemDto> Locations { get; set; } = new List<CreateLocationItemDto>();
    public required CreateCategoryDto Category { get; set; }

    public async Task<IProduct> UnpackDto(IEntityQueryService<Category, SearchableCategory> categoryService)
    {
        ICategory category = await Category.UnpackDto(categoryService);
        IList<ILocationItem> locationItems = Locations.Select(dto => dto.UnpackDto()).ToList();

        return new Product() {Category = category, Locations = locationItems, Name = Name, Price = Price};
    }
}
