using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Services;

public class ProductQueryService : BaseEntityQueryService<InventoryDatabaseContext, Product, SearchableProduct>
{
    /// <inheritdoc />
    public ProductQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<Product> AddComplexQueryArguments(IQueryable<Product> query, IComplexSearchable<SearchableProduct> complex)
    {
        if (complex is not ComplexSearchableProduct complexSearchableProduct)
        {
            throw new ArgumentException(
                $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableProduct)}, but it wasn't.");
        }

        query = AddLocationRelatedQueryArguments(query, complexSearchableProduct);

        if (complexSearchableProduct.IncludeOrders.HasValue)
        {
            query = query.Include(x => x.Orders).ThenInclude(x => x.Order);
        }

        if (!string.IsNullOrWhiteSpace(complexSearchableProduct.CategoryNameContains))
        {
            string keyword = $"%{complexSearchableProduct.CategoryNameContains}%";
            query = query.Where(x => EF.Functions.Like(x.Category.Name, keyword));
        }

        if (!string.IsNullOrWhiteSpace(complexSearchableProduct.ProductNameContains))
        {
            string keyword = $"%{complexSearchableProduct.ProductNameContains}%";
            query = query.Where(x => EF.Functions.Like(x.Name, keyword));
        }

        return query;
    }

    /// <inheritdoc />
    protected override IEnumerable<Product> ApplyComplexNonDatabaseQueryArguments(IEnumerable<Product> entities, IComplexSearchable<SearchableProduct> complex)
    {
        if (complex is not ComplexSearchableProduct complexSearchableProduct)
        {
            throw new ArgumentException(
                $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableProduct)}, but it wasn't.");
        }

        if (complexSearchableProduct is {IncludeLocations: not null, HasInventoryStatus: not null})
        {
            entities = entities.Where(x => x.Locations.Any(loc =>
                loc.Status.ToString() == complexSearchableProduct.HasInventoryStatus.ToString()));
        }

        return entities;
    }

    private IQueryable<Product> AddLocationRelatedQueryArguments(
        IQueryable<Product> query, ComplexSearchableProduct complexSearchableProduct)
    {
        if (!complexSearchableProduct.IncludeLocations.HasValue)
        {
            return query;
        }

        query = query.Include(x => x.Locations).ThenInclude(x=> x.Location);

        if (!string.IsNullOrWhiteSpace(complexSearchableProduct.LocationNameContains))
        {
            string keyword = $"%{complexSearchableProduct.LocationNameContains}%";
            query = query.Where(x =>
                x.Locations.Any(loc => EF.Functions.Like(loc.Location.Name, keyword)));
        }

        return query;
    }

    /// <inheritdoc />
    protected override IQueryable<Product> GetBaseQuery()
    {
        return context.Products.AsQueryable().Include(x => x.Category);
    }

    /// <inheritdoc />
    protected override IQueryable<Product> AddQueryArguments(SearchableProduct searchable, IQueryable<Product> query)
    {
        if (searchable.Price != 0)
        {
            query = query.Where(x => x.Price == searchable.Price);
        }

        if (!string.IsNullOrWhiteSpace(searchable.Name))
        {
            query = query.Where(x => x.Name.ToLower() == searchable.Name.ToLower());
        }

        return query;
    }
}
