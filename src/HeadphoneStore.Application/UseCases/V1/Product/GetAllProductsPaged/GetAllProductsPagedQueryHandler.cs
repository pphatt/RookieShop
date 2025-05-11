using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;
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
        var query = await _productRepository.GetAllProductsPagination(
            categoryIds: request.CategoryIds,
            keyword: request.SearchTerm,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        var resultItems = query.Items.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug,
            Description = x.Description,
            Stock = x.Stock,
            Sold = x.Sold,
            Sku = x.Sku,
            Category = new CategoryDto
            {
                Id = x.Category.Id,
                Name = x.Category.Name
            },
            Brand = new BrandDto
            {
                Id = x.Brand.Id,
                Name = x.Brand.Name,
                Slug = x.Brand.Slug
            },
            ProductStatus = x.ProductStatus.ToString(),
            ProductPrice = x.ProductPrice.Amount,
            AverageRating = x.AverageRating,
            TotalReviews = x.TotalReviews,
            Status = x.Status.ToString(),
            Media = x.Media.OrderBy(x => x.DisplayOrder).Select(x => new ProductMediaDto
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Path = x.Path,
                PublicId = x.PublicId,
                DIsplayOrder = x.DisplayOrder
            }).ToList().AsReadOnly()
        }).ToList();

        return Result.Success(new PagedResult<ProductDto>(resultItems, query.PageIndex, query.PageSize, query.TotalCount));
    }
}
