using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;

using Exceptions = Domain.Exceptions.Exceptions;

public class DeleteBrandCommandHandler : ICommandHandler<DeleteBrandCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBrandRepository _brandRepository;
    private readonly ICacheService _cacheService;

    public DeleteBrandCommandHandler(
        UserManager<AppUser> userManager,
        IUnitOfWork unitOfWork,
        IBrandRepository brandRepository,
        ICacheService cacheService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _brandRepository = brandRepository;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UpdatedBy.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var brand = _brandRepository.FindByCondition(x => x.Id == request.Id).SingleOrDefault();

        if (brand is null)
            throw new Exceptions.Brand.NotFound();

        if (brand.IsDeleted)
            throw new Exceptions.Brand.AlreadyDeleted();

        brand.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Brands", cancellationToken);

        return Result.Success("Delete brand successfully.");
    }
}
