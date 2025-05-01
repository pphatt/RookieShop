using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Repositories;

public class BrandRepository : RepositoryBase<Brand, Guid>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<BrandDto>> GetBrandsPagination(string? keyword, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        query = query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDateTime);

        var result = query.Select(x => new BrandDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Status = x.Status.ToString(),
        });

        return await PagedResult<BrandDto>.InitializeAsync(result, pageIndex, pageSize);
    }
}
