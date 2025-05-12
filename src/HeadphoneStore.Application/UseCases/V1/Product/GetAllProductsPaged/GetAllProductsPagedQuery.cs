using System.Text;

using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;

public sealed record GetAllProductsPagedQuery(List<Guid>? CategoryIds,
                                      string? SearchTerm,
                                      int PageIndex,
                                      int PageSize) : IQuery<PagedResult<ProductDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append($"Products:{nameof(GetAllProductsPagedQuery)}");

            if (SearchTerm != null)
            {
                builder.Append($":SearchTerm:{SearchTerm}");
            }

            if (CategoryIds is not null && CategoryIds.Any())
            {
                builder.Append($":Category:");

                var ids = new StringBuilder();

                foreach (var id in CategoryIds)
                {
                    ids.Append(id);
                }

                builder.Append(string.Join("-", ids));
            }

            builder.Append($":Page:{PageIndex}:{PageSize}");

            return builder.ToString();
        }
    }
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
