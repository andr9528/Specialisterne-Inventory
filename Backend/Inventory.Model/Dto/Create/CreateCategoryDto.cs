using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;

namespace Inventory.Model.Dto.Create;

public class CreateCategoryDto
{
    public required string Name { get; set; }

    public async Task<ICategory> UnpackDto(IEntityQueryService<Category, SearchableCategory> categoryService)
    {
        ICategory? existing = await categoryService.GetEntityComplex(new ComplexSearchableCategory()
            {CategoryNameContains = Name});

        if (existing == null)
        {
            return new Category() {Name = Name};
        }

        return existing;
    }
}
