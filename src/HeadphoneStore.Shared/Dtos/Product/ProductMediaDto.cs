namespace HeadphoneStore.Shared.Dtos.Product;

public class ProductMediaDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public string PublicId { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public int DIsplayOrder { get; set; }
}
