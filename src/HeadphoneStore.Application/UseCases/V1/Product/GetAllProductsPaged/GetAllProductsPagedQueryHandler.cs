using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;

public class GetAllProductsPagedQueryHandler : IQueryHandler<GetAllProductsPagedQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsPagedQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PagedResult<ProductDto>>> Handle(GetAllProductsPagedQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetAllProductPagination(
            categorySlug: request.CategorySlug,
            keyword: request.SearchTerm,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        result.Items = result.Items.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug,
            Description = x.Description,
            Stock = x.Stock,
            Sold = x.Sold,
            Sku = x.Sku,
            Category = x.Category,
            Brand = x.Brand,
            ProductStatus = x.ProductStatus.ToString().FormatPascalCaseString(),
            ProductPrice = x.ProductPrice,
            AverageRating = x.AverageRating,
            TotalReviews = x.TotalReviews,
            Status = x.Status,
            Media = x.Media
        }).ToList();

        return Result.Success(result);
    }
}
