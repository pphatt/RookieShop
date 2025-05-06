using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.StoreFrontEnd.Models.Cart;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    
    public int TotalCartItems { get; set; }
    
    public decimal TotalCartPrice { get; set; }
}

public class CartItemViewModel
{
    public Guid ProductId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public int Quantity { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public CategoryDto Category { get; set; } = null!;
    
    public BrandDto Brand { get; set; } = null!;
    
    public decimal Total => Price * Quantity;
}
