using System.Reflection;
using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Model.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inventory.Server.JsonConverters;

public class ProductJsonConverter : JsonConverter<Product>
{
    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, Product? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        WriteSimpleProperties(writer, value, serializer);
        WriteCategory(writer, value, serializer);
        WriteLocationItems(writer, value, serializer);
        WriteOrderItems(writer, value, serializer);

        writer.WriteEndObject();
    }

    private void WriteCategory(JsonWriter writer, Product product, JsonSerializer serializer)
    {
        writer.WritePropertyName(nameof(Product.Category));

        if (product.Category is null)
        {
            writer.WriteNull();
            return;
        }

        var category = product.Category as Category ?? throw new JsonSerializationException(
            $"{nameof(Product.Category)} must be of type {nameof(Category)} when serializing {nameof(Product)}.");

        writer.WriteStartObject();

        var categoryProperties = typeof(Category).GetProperties()
            .Where(property => property.CanRead && IsSimpleType(property.PropertyType));

        foreach (var property in categoryProperties)
        {
            writer.WritePropertyName(property.Name);
            serializer.Serialize(writer, property.GetValue(category));
        }

        writer.WriteEndObject();
    }

    private void WriteOrderItems(JsonWriter writer, Product product, JsonSerializer serializer)
    {
        writer.WritePropertyName(nameof(Product.Orders));
        writer.WriteStartArray();

        foreach (OrderItem orderItem in product.Orders.Cast<OrderItem>())
        {
            WriteOrderItem(writer, orderItem, serializer);
        }

        writer.WriteEndArray();
    }

    private void WriteOrderItem(JsonWriter writer, OrderItem orderItem, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        WriteOrderItemSimpleProperties(writer, orderItem, serializer);
        WriteOrder(writer, orderItem.Order as Order, serializer);

        writer.WriteEndObject();
    }

    private void WriteOrderItemSimpleProperties(JsonWriter writer, OrderItem orderItem, JsonSerializer serializer)
    {
        var orderItemProperties = typeof(OrderItem)
            .GetProperties()
            .Where(property =>
                property.CanRead &&
                property.CanWrite &&
                property.Name != nameof(OrderItem.Product) &&
                property.Name != nameof(OrderItem.Order) &&
                IsSimpleType(property.PropertyType));

        foreach (var property in orderItemProperties)
        {
            writer.WritePropertyName(property.Name);
            serializer.Serialize(writer, property.GetValue(orderItem));
        }
    }

    private void WriteOrder(JsonWriter writer, Order? order, JsonSerializer serializer)
    {
        writer.WritePropertyName(nameof(OrderItem.Order));

        if (order is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        var orderProperties = typeof(Order)
            .GetProperties()
            .Where(property =>
                property.CanRead &&
                property.CanWrite &&
                property.Name != nameof(Order.Products) &&
                property.Name != nameof(Order.Location) &&
                IsSimpleType(property.PropertyType));

        foreach (var property in orderProperties)
        {
            writer.WritePropertyName(property.Name);
            serializer.Serialize(writer, property.GetValue(order));
        }

        writer.WriteEndObject();
    }

    private void WriteSimpleProperties(JsonWriter writer, Product product, JsonSerializer serializer)
    {
        var properties = typeof(Product)
            .GetProperties()
            .Where(property =>
                property.CanRead &&
                property.Name != nameof(Product.Locations) &&
                property.Name != nameof(Product.Orders) &&
                IsSimpleType(property.PropertyType));

        foreach (var property in properties)
        {
            writer.WritePropertyName(property.Name);
            serializer.Serialize(writer, property.GetValue(product));
        }
    }

    private void WriteLocationItems(JsonWriter writer, Product product, JsonSerializer serializer)
    {
        writer.WritePropertyName(nameof(Product.Locations));
        writer.WriteStartArray();

        foreach (var locationItem in product.Locations)
        {
            WriteLocationItem(writer, locationItem, serializer);
        }

        writer.WriteEndArray();
    }

    private void WriteLocationItem(JsonWriter writer, ILocationItem locationItem, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        WriteLocationItemSimpleProperties(writer, locationItem, serializer);
        WriteLocation(writer, locationItem.Location, serializer);

        writer.WriteEndObject();
    }

    private void WriteLocationItemSimpleProperties(JsonWriter writer, ILocationItem locationItem, JsonSerializer serializer)
    {
        var locationItemProperties = typeof(LocationItem)
            .GetProperties()
            .Where(property =>
                property.CanRead &&
                property.Name != nameof(LocationItem.Product) &&
                property.Name != nameof(LocationItem.Location) &&
                IsSimpleType(property.PropertyType));

        foreach (var property in locationItemProperties)
        {
            writer.WritePropertyName(property.Name);
            serializer.Serialize(writer, property.GetValue(locationItem));
        }
    }

    private void WriteLocation(JsonWriter writer, ILocation? location, JsonSerializer serializer)
    {
        writer.WritePropertyName(nameof(LocationItem.Location));

        if (location is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        var locationProperties = typeof(Location)
            .GetProperties()
            .Where(property =>
                property.CanRead &&
                property.CanWrite &&
                property.Name != nameof(Location.Products) &&
                property.Name != nameof(Location.Orders) &&
                IsSimpleType(property.PropertyType));

        foreach (var property in locationProperties)
        {
            writer.WritePropertyName(property.Name);
            serializer.Serialize(writer, property.GetValue(location));
        }

        writer.WriteEndObject();
    }

    private bool IsSimpleType(Type type)
    {
        Type actualType = Nullable.GetUnderlyingType(type) ?? type;

        return actualType.IsPrimitive ||
               actualType.IsEnum ||
               actualType == typeof(string) ||
               actualType == typeof(decimal) ||
               actualType == typeof(DateTime) ||
               actualType == typeof(DateTimeOffset) ||
               actualType == typeof(TimeSpan) ||
               actualType == typeof(Guid);
    }

    public override Product? ReadJson(
        JsonReader reader, Type objectType, Product? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        JObject jsonObject = JObject.Load(reader);

        int id = jsonObject[nameof(Product.Id)]?.ToObject<int>(serializer) ?? 0;

        Product product = CreateProduct(jsonObject, id, serializer);

        ReadSimpleProperties(jsonObject, product, serializer);
        ReadCategory(jsonObject, product, serializer);
        ReadLocationItems(jsonObject, product, serializer);
        ReadOrderItems(jsonObject, product, serializer);

        return product;
    }

    private void ReadOrderItems(JObject jsonObject, Product product, JsonSerializer serializer)
    {
        JArray? ordersArray = jsonObject[nameof(Product.Orders)] as JArray;
        if (ordersArray is null)
        {
            return;
        }

        product.Orders.Clear();

        foreach (JObject orderItemObject in ordersArray.OfType<JObject>())
        {
            OrderItem orderItem = CreateOrderItem(orderItemObject, product, serializer);
            product.Orders.Add(orderItem);
        }
    }

    private OrderItem CreateOrderItem(JObject orderItemObject, Product product, JsonSerializer serializer)
    {
        int id = orderItemObject[nameof(OrderItem.Id)]?.ToObject<int>(serializer) ?? 0;

        Order order = CreateOrder(orderItemObject, serializer) ?? new Order();

        OrderItem orderItem = (OrderItem) Activator.CreateInstance(typeof(OrderItem),
            BindingFlags.Instance | BindingFlags.NonPublic, binder: null, args: new object[] {id, product, order},
            culture: null)!;

        ReadOrderItemSimpleProperties(orderItemObject, orderItem, serializer);

        return orderItem;
    }

    private void ReadOrderItemSimpleProperties(JObject orderItemObject, OrderItem orderItem, JsonSerializer serializer)
    {
        var properties = typeof(OrderItem).GetProperties().Where(property =>
            property.CanRead && property.CanWrite && property.Name != nameof(OrderItem.Id) &&
            property.Name != nameof(OrderItem.Product) && property.Name != nameof(OrderItem.Order) &&
            IsSimpleType(property.PropertyType));

        foreach (var property in properties)
        {
            JToken? token = orderItemObject[property.Name];

            if (token is null || token.Type == JTokenType.Null)
            {
                continue;
            }

            object? value = token.ToObject(property.PropertyType, serializer);
            property.SetValue(orderItem, value);
        }
    }

    private Order? CreateOrder(JObject orderItemObject, JsonSerializer serializer)
    {
        JObject? orderObject = orderItemObject[nameof(OrderItem.Order)] as JObject;
        if (orderObject is null)
        {
            return null;
        }

        int id = orderObject[nameof(Order.Id)]?.ToObject<int>(serializer) ?? 0;

        List<OrderItem> products = []; // empty list (no back-reference population)

        Order order = (Order) Activator.CreateInstance(typeof(Order), BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null, args: new object[] {id, products}, culture: null)!;

        ReadOrderSimpleProperties(orderObject, order, serializer);

        return order;
    }

    private void ReadOrderSimpleProperties(JObject orderObject, Order order, JsonSerializer serializer)
    {
        var properties = typeof(Order).GetProperties().Where(property =>
            property.CanRead && property.CanWrite && property.Name != nameof(Order.Id) &&
            property.Name != nameof(Order.Products) && property.Name != nameof(Order.Location) &&
            IsSimpleType(property.PropertyType));

        foreach (var property in properties)
        {
            JToken? token = orderObject[property.Name];

            if (token is null || token.Type == JTokenType.Null)
            {
                continue;
            }

            object? value = token.ToObject(property.PropertyType, serializer);
            property.SetValue(order, value);
        }
    }

    private Product CreateProduct(JObject jsonObject, int id, JsonSerializer serializer)
    {
        List<LocationItem> locations = [];
        List<OrderItem> orders = [];
        Category? category = ResolveCategory(jsonObject, serializer);

        return (Product) Activator.CreateInstance(typeof(Product), BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null, args: new object?[] {id, locations, orders, category!}, culture: null)!;
    }

    private Category? ResolveCategory(JObject jsonObject, JsonSerializer serializer)
    {
        JToken? categoryToken = jsonObject[nameof(Product.Category)];
        if (categoryToken is JObject categoryObject)
        {
            return categoryObject.ToObject<Category>(serializer);
        }

        int categoryId = jsonObject[nameof(Product.CategoryId)]?.ToObject<int>(serializer) ?? 0;

        if (categoryId > 0)
        {
            return null;
        }

        return new Category();
    }

    private void ReadSimpleProperties(JObject jsonObject, Product product, JsonSerializer serializer)
    {
        var properties = typeof(Product).GetProperties().Where(property =>
            property.CanRead && property.CanWrite && property.Name != nameof(Product.Id) &&
            property.Name != nameof(Product.Category) && property.Name != nameof(Product.Locations) &&
            property.Name != nameof(Product.Orders) && IsSimpleType(property.PropertyType));

        foreach (var property in properties)
        {
            JToken? token = jsonObject[property.Name];

            if (token is null || token.Type == JTokenType.Null)
            {
                continue;
            }

            object? value = token.ToObject(property.PropertyType, serializer);
            property.SetValue(product, value);
        }
    }

    private void ReadCategory(JObject jsonObject, Product product, JsonSerializer serializer)
    {
        product.Category = ResolveCategory(jsonObject, serializer)!;
    }

    private void ReadLocationItems(JObject jsonObject, Product product, JsonSerializer serializer)
    {
        JArray? locationsArray = jsonObject[nameof(Product.Locations)] as JArray;
        if (locationsArray is null)
        {
            return;
        }

        product.Locations.Clear();

        foreach (JObject locationItemObject in locationsArray.OfType<JObject>())
        {
            LocationItem locationItem = CreateLocationItem(locationItemObject, product, serializer);
            product.Locations.Add(locationItem);
        }
    }

    private LocationItem CreateLocationItem(JObject locationItemObject, Product product, JsonSerializer serializer)
    {
        LocationItem locationItem = CreateInstance<LocationItem>();

        PopulateLocationItemSimpleProperties(locationItemObject, locationItem, serializer);

        locationItem.Product = product;
        locationItem.Location = CreateLocation(locationItemObject, serializer);

        return locationItem;
    }

    private void PopulateLocationItemSimpleProperties(
        JObject locationItemObject,
        LocationItem locationItem,
        JsonSerializer serializer)
    {
        var properties = typeof(LocationItem)
            .GetProperties()
            .Where(property =>
                property.CanRead &&
                property.CanWrite &&
                property.Name != nameof(LocationItem.Product) &&
                property.Name != nameof(LocationItem.Location) &&
                IsSimpleType(property.PropertyType));

        foreach (var property in properties)
        {
            JToken? token = locationItemObject[property.Name];

            if (token is null || token.Type == JTokenType.Null)
            {
                continue;
            }

            object? value = token.ToObject(property.PropertyType, serializer);
            property.SetValue(locationItem, value);
        }
    }

    private Location? CreateLocation(JObject locationItemObject, JsonSerializer serializer)
    {
        JObject? locationObject = locationItemObject[nameof(LocationItem.Location)] as JObject;
        if (locationObject is null)
        {
            return null;
        }

        Location location = CreateInstance<Location>();

        var properties = typeof(Location)
            .GetProperties()
            .Where(property =>
                property.CanRead &&
                property.CanWrite &&
                property.Name != nameof(Location.Products) &&
                property.Name != nameof(Location.Orders) &&
                IsSimpleType(property.PropertyType));

        foreach (var property in properties)
        {
            JToken? token = locationObject[property.Name];

            if (token is null || token.Type == JTokenType.Null)
            {
                continue;
            }

            object? value = token.ToObject(property.PropertyType, serializer);
            property.SetValue(location, value);
        }

        return location;
    }

    private T CreateInstance<T>() where T : class
    {
        return Activator.CreateInstance(typeof(T)) as T
               ?? throw new JsonSerializationException(
                   $"Could not create instance of type {typeof(T).FullName}.");
    }
}
