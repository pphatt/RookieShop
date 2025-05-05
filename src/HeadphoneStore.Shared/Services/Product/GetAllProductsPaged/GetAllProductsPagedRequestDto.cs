using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Shared.Services.Product.GetAllPaged;

public class GetAllProductPagedRequestDto : PagedRequestDto
{
    public List<Guid>? CategoryIds { get; set; }
}
