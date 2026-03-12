namespace Inventory.Abstraction.Enum;

public enum OrderStatus
{
    UNKNOWN = 0, // Default Value - Checked for in Query Service
    OPEN = 1,
    CLOSED = 100,
}
