using Inventory.Abstraction.Interfaces.Model.Searchable;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

internal interface ICategory : ISearchableCategory, IEntity
{
}
