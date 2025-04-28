using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Product;
using HeadphoneStore.Domain.Abstracts.Repositories;

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
            keyword: request.SearchTerm,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        result.Items = result.Items.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Quantity = x.Quantity,
            Sku = x.Sku,
            Category = x.Category,
            Brand = x.Brand,
            ProductStatus = x.ProductStatus.ToString().FormatPascalCaseString(),
            ProductPrice = x.ProductPrice,
            AverageRating = x.AverageRating,
            TotalReviews = x.TotalReviews,
            Media = x.Media
        }).ToList();

        return Result.Success(result);
    }
}
