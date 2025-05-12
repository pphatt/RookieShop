using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Shared.Services.Product.GetAllPaged;

public class GetAllProductsPagedRequestDto : PagedRequestDto
{
    public List<Guid>? CategoryIds { get; set; }
}
