using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Brand.ActiveBrand;

using Exceptions = Domain.Exceptions.Exceptions;

public class ActivateBrandCommandHandler : ICommandHandler<ActivateBrandCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBrandRepository _brandRepository;
    private readonly ICacheService _cacheService;

    public ActivateBrandCommandHandler(IUnitOfWork unitOfWork, IBrandRepository brandRepository, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _brandRepository = brandRepository;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(ActivateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.FindByIdAsync(request.Id);

        if (brand is null)
            throw new Exceptions.Brand.NotFound();

        if (brand.IsDeleted)
            throw new Exceptions.Brand.AlreadyDeleted();

        if (brand.Status == EntityStatus.Active)
            throw new Exceptions.Brand.AlreadyActive();

        brand.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Brands", cancellationToken);

        return Result.Success("Active brand successfully.");
    }
}
