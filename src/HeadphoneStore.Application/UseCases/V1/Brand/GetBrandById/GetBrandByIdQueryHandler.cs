using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;

using Exceptions = Domain.Exceptions.Exceptions;

public class GetBrandByIdQueryHandler : ICommandHandler<GetBrandByIdQuery, BrandDto>
{
    private readonly IBrandRepository _brandRepository;

    public GetBrandByIdQueryHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.FindByIdAsync(request.Id);

        if (brand is null || brand.IsDeleted)
            throw new Exceptions.Brand.NotFound();

        var result = new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Slug = brand.Slug,
            Description = brand.Description,
            Status = brand.Status.ToString(),
        };

        return Result.Success(result);
    }
}
