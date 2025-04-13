//using HeadphoneStore.Domain.Abstracts.Entities;
//using HeadphoneStore.Domain.Aggregates.Categories.Entities;

//namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

//public class Product : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
//{
//    public Guid CategoryId { get; set; }
//    public string Name { get; private set; }
//    public decimal Price { get; private set; }
//    public string Description { get; private set; }
//    public int Sku { get; private set; }
//    public int Sold { get; private set; }
//    public Guid CreatedBy { get; set; }
//    public Guid? UpdatedBy { get; set; }

//    public Category Category { get; set; }
//}
