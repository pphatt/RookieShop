using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Brand;
using HeadphoneStore.Domain.Abstracts.Repositories;

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
            Description = brand.Description,
        };

        return Result.Success(result);
    }
}
