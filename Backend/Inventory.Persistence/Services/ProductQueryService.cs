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
            query = query.Include(x => x.Orders);
        }

        if (!string.IsNullOrWhiteSpace(complexSearchableProduct.CategoryNameContains))
        {
            string keyword = $"%{complexSearchableProduct.CategoryNameContains}%";
            query = query.Where(x => EF.Functions.Like(x.Category.Name, keyword));
        }

        return query;
    }

    private IQueryable<Product> AddLocationRelatedQueryArguments(
        IQueryable<Product> query, ComplexSearchableProduct complexSearchableProduct)
    {
        if (!complexSearchableProduct.IncludeLocations.HasValue)
        {
            return query;
        }

        var includedQuery = query.Include(x => x.Locations);

        if (!string.IsNullOrWhiteSpace(complexSearchableProduct.LocationNameContains))
        {
            string keyword = $"%{complexSearchableProduct.LocationNameContains}%";
            query = includedQuery.ThenInclude(x => x.Location).Where(x =>
                x.Locations.Any(loc => EF.Functions.Like(loc.Location.Name, keyword)));
        }
        else
        {
            query = includedQuery;
        }

        if (complexSearchableProduct.HasInventoryStatus.HasValue)
        {
            query = query.Where(x =>
                x.Locations.Any(loc => loc.Status == complexSearchableProduct.HasInventoryStatus));
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
