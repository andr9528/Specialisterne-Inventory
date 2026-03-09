using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;

namespace Inventory.Persistence.Services
{
    public class ExampleQueryService : BaseEntityQueryService<ExampleDatabaseContext, Example, SearchableExample>
    {
        /// <inheritdoc />
        public ExampleQueryService(ExampleDatabaseContext context) : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<Example> GetBaseQuery()
        {
            return context.Examples.AsQueryable();
        }

        /// <inheritdoc />
        protected override IQueryable<Example> AddQueryArguments(
            SearchableExample searchable, IQueryable<Example> query)
        {
            // Example of how to add query arguments based on the SearchableExample int property.
            //if (searchable.SomeInt != 0)
            //{
            //    query = query.Where(x => x.SomeInt == searchable.SomeInt);
            //}

            // Example of how to add query arguments based on the SearchableExample Enum property.
            //if (!Equals(searchable.SomeEnum, default(SomeEnumType)))
            //{
            //    query = query.Where(x => x.SomeEnum == searchable.SomeEnum);
            //}

            // Example of how to add query arguments based on the SearchableExample char property.
            //if (searchable.SomeChar != '\0')
            //{
            //    query = query.Where(x => char.ToLower(x.SomeChar) == char.ToLower(searchable.SomeChar));
            //}

            // Example of how to add query arguments based on the SearchableExample string property.
            //if (!string.IsNullOrWhiteSpace(searchable.SomeString))
            //{
            //    query = query.Where(x => x.SomeString.ToLower() == searchable.SomeString.ToLower());
            //}

            return query;
        }
    }
}