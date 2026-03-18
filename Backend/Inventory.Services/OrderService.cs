using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Abstraction.Interfaces.Services;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Microsoft.Extensions.Logging;

namespace Inventory.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> logger;
    private readonly IEntityQueryService<LocationItem, SearchableLocationItem> locationItemQueryService;

    public OrderService(ILogger<OrderService> logger, IEntityQueryService<LocationItem, SearchableLocationItem> locationItemQueryService)
    {
        this.logger = logger;
        this.locationItemQueryService = locationItemQueryService;
    }

    /// <inheritdoc />
    public async Task<IList<IOrder>> CreateOrders(Guid reference, IEnumerable<IOrderItem> items, int? locationId)
    {
        ArgumentNullException.ThrowIfNull(items);

        List<IOrderItem> requestedItems = items.ToList();
        ValidateItems(requestedItems);

        Dictionary<int, LocationReservation> reservations = await AllocateItemsToLocations(requestedItems, locationId);

        // Todo: Don't throw, but somehow indicate that the order is awaiting fulfillment.
        // Double check that nothing was missed.
        EnsureAllItemsWereAllocated(reservations.Values, requestedItems);

        await ApplyReservations(reservations.Values);

        return CreateOrders(reference, reservations.Values);
    }

    private class LocationReservation
    {
        /// <summary>
        /// Gets the location id this reservation belongs to.
        /// </summary>
        public int LocationId { get; init; }

        /// <summary>
        /// Gets the allocated order items for this location.
        /// </summary>
        public List<IOrderItem> Items { get; } = new();

        /// <summary>
        /// Gets the backing location item entities keyed by product id.
        /// </summary>
        public Dictionary<int, ILocationItem> LocationItemsByProductId { get; } = new();
    }

    /// <summary>
    /// Allocates requested items across locations, optionally preferring one location first.
    /// </summary>
    private async Task<Dictionary<int, LocationReservation>> AllocateItemsToLocations(
        IReadOnlyList<IOrderItem> requestedItems, int? preferredLocationId)
    {
        var reservations = new Dictionary<int, LocationReservation>();
        var usedLocationIds = new HashSet<int>();

        foreach (IOrderItem requestedItem in requestedItems)
        {
            IList<ILocationItem> possibleLocations = await GetPossibleLocations(requestedItem);

            AllocateSingleItem(requestedItem, possibleLocations, reservations, usedLocationIds, preferredLocationId);
        }

        return reservations;
    }

    /// <summary>
    /// Allocates one requested item across one or more locations, using the preferred location first when possible.
    /// </summary>
    private void AllocateSingleItem(
        IOrderItem requestedItem, IList<ILocationItem> possibleLocations,
        Dictionary<int, LocationReservation> reservations, HashSet<int> usedLocationIds, int? preferredLocationId)
    {
        int remainingQuantity = requestedItem.Quantity;

        foreach (ILocationItem locationItem in OrderLocationsForAllocation(possibleLocations, usedLocationIds,
                     preferredLocationId))
        {
            if (remainingQuantity == 0)
                break;

            int allocatedQuantity = Math.Min(remainingQuantity, locationItem.Quantity);
            if (allocatedQuantity <= 0)
                continue;

            AddAllocatedItem(reservations, locationItem, requestedItem.ProductId, allocatedQuantity);
            usedLocationIds.Add(locationItem.LocationId);
            remainingQuantity -= allocatedQuantity;
        }

        EnsureItemWasFullyAllocated(requestedItem, remainingQuantity);
    }

    /// <summary>
    /// Orders candidate locations by preferred location first, then already-used locations, then highest stock.
    /// </summary>
    public IEnumerable<ILocationItem> OrderLocationsForAllocation(
        IEnumerable<ILocationItem> possibleLocations, IReadOnlyCollection<int> usedLocationIds,
        int? preferredLocationId)
    {
        return possibleLocations
            .OrderByDescending(x => preferredLocationId is not null && x.LocationId == preferredLocationId.Value)
            .ThenByDescending(x => usedLocationIds.Contains(x.LocationId)).ThenByDescending(x => x.Quantity)
            .ThenBy(x => x.LocationId);
    }

    /// <summary>
    /// Adds an allocated item to the reservation bucket for one location.
    /// </summary>
    private void AddAllocatedItem(
        Dictionary<int, LocationReservation> reservations, ILocationItem locationItem, int productId, int quantity)
    {
        if (!reservations.TryGetValue(locationItem.LocationId, out LocationReservation? reservation))
        {
            reservation = new LocationReservation
            {
                LocationId = locationItem.LocationId
            };

            reservations.Add(locationItem.LocationId, reservation);
        }

        reservation.Items.Add(new OrderItem
        {
            ProductId = productId,
            Quantity = quantity
        });

        reservation.LocationItemsByProductId[productId] = locationItem;
    }

    /// <summary>
    /// Throws when the requested item could not be fully allocated across all available locations.
    /// </summary>
    private void EnsureItemWasFullyAllocated(IOrderItem requestedItem, int remainingQuantity)
    {
        if (remainingQuantity == 0)
            return;

        // Todo: Don't throw, but somehow indicate that the order is awaiting fulfillment.
        throw new InvalidOperationException(
            $"Unable to fully allocate product {requestedItem.ProductId}. Missing quantity: {remainingQuantity}.");
    }

    /// <summary>
    /// Validates that the request contains at least one valid order item.
    /// </summary>
    private void ValidateItems(IReadOnlyCollection<IOrderItem> items)
    {
        if (!items.Any())
        {
            throw new ArgumentException("At least one order item is required.", nameof(items));
        }

        if (items.Any(x => x.Quantity <= 0))
        {
            throw new ArgumentException("All order items must have a quantity above 0.", nameof(items));
        }
    }

    /// <summary>
    /// Retrieves all locations that can fulfill the full quantity of a single order item.
    /// </summary>
    private async Task<IList<ILocationItem>> GetPossibleLocations(IOrderItem item)
    {
        var search = new ComplexSearchableLocationItem
        {
            MinimumItemsInStock = 1,
            Searchable = new SearchableLocationItem
            {
                ProductId = item.ProductId
            }
        };

        return (await locationItemQueryService.GetEntitiesComplex(search)).ToList<ILocationItem>();
    }

    /// <summary>
    /// Ensures that all requested items were fully allocated across the selected locations.
    /// </summary>
    private void EnsureAllItemsWereAllocated(
        IEnumerable<LocationReservation> reservations, IReadOnlyCollection<IOrderItem> requestedItems)
    {
        Dictionary<int, int> requestedQuantities = SumQuantitiesByProduct(requestedItems);
        Dictionary<int, int> allocatedQuantities = SumQuantitiesByProduct(reservations.SelectMany(x => x.Items));

        foreach ((int productId, int requestedQuantity) in requestedQuantities)
        {
            allocatedQuantities.TryGetValue(productId, out int allocatedQuantity);

            if (allocatedQuantity < requestedQuantity)
            {
                throw new InvalidOperationException(
                    $"Unable to fully allocate product {productId}. Missing quantity: {requestedQuantity - allocatedQuantity}.");
            }
        }
    }

    /// <summary>
    /// Sums quantities by product id.
    /// </summary>
    public Dictionary<int, int> SumQuantitiesByProduct(IEnumerable<IOrderItem> items)
    {
        return items.GroupBy(x => x.ProductId).ToDictionary(group => group.Key, group => group.Sum(x => x.Quantity));
    }

    /// <summary>
    /// Moves allocated quantities from Quantity to ReservedQuantity and saves the updated location items.
    ///</summary>
    private async Task ApplyReservations(IEnumerable<LocationReservation> reservations)
    {
        List<LocationItem> updatedLocationItems = new();

        foreach (LocationReservation reservation in reservations)
        {
            foreach (IOrderItem item in reservation.Items)
            {
                ILocationItem locationItem = reservation.LocationItemsByProductId[item.ProductId];

                if (locationItem.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {item.ProductId} at location {reservation.LocationId}.");
                }

                locationItem.Quantity -= item.Quantity;
                locationItem.ReservedQuantity += item.Quantity;
            }

            foreach (ILocationItem locationItem in reservation.LocationItemsByProductId.Values)
            {
                if (updatedLocationItems.All(x => x.Id != locationItem.Id))
                {
                    updatedLocationItems.Add((LocationItem) locationItem);
                }
            }
        }

        await locationItemQueryService.UpdateEntities(updatedLocationItems);
    }

    /// <summary>
    /// Creates one order per location from the generated reservations.
    /// </summary>
    private IList<IOrder> CreateOrders(Guid reference, IEnumerable<LocationReservation> reservations)
    {
        return reservations.Select(x => CreateSingleOrder(reference, x.LocationId, x.Items)).Cast<IOrder>().ToList();
    }

    /// <summary>
    /// Creates a single order for one location.
    /// </summary>
    private Order CreateSingleOrder(Guid reference, int locationId, IReadOnlyCollection<IOrderItem> items)
    {
        return new Order
        {
            ReferenceId = reference,
            LocationId = locationId,
            Products = items.ToList(),
            Status = OrderStatus.OPEN,
        };
    }
}
