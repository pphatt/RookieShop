using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Shared.Dtos.Order;

public class CreateOrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class OrderDetailDto
{
    public ProductDto Product { get; set; }
    public int Quantity { get; set; } = 1;
}
