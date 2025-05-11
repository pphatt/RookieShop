using System.Text;

using HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Order;

namespace HeadphoneStore.Application.UseCases.V1.Order.GetAllOrdersPagination;

public class GetAllOrdersPaginationQuery : PagedDto, IQuery<PagedResult<OrderDto>>, ICacheable
{
    public Guid UserId { get; set; }

    public bool BypassCache => false;
    public string CacheKey
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append($"Products:{nameof(GetAllProductsPagedQuery)}");
            builder.Append($":{UserId}");

            if (SearchTerm != null)
            {
                builder.Append($":SearchTerm:{SearchTerm}");
            }

            builder.Append($":Page:{PageIndex}:{PageSize}");

            return builder.ToString();
        }
    }
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
