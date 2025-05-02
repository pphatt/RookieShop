using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateBrandCommandHandler : ICommandHandler<UpdateBrandCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateBrandCommandHandler(
        UserManager<AppUser> userManager,
        IBrandRepository brandRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _userManager = userManager;
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UpdatedBy.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var brandFromDb = await _brandRepository.FindByIdAsync(request.Id);

        if (brandFromDb is null)
            throw new Exceptions.Brand.NotFound();

        var duplicateName = _brandRepository.FindByCondition(x => x.Name.Equals(request.Name)).FirstOrDefault();

        if (duplicateName is not null && duplicateName.Id != brandFromDb.Id)
            throw new Exceptions.Brand.DuplicateName();

        var slug = brandFromDb.Name != request.Name ? request.Name.Slugify() : request.Slug;
        var isSlugAlreadyExisted = await _brandRepository.IsSlugAlreadyExisted(slug, brandFromDb.Id);

        if (isSlugAlreadyExisted)
            throw new Exceptions.Brand.SlugExists();

        if (brandFromDb.IsDeleted)
            throw new Exceptions.Brand.AlreadyDeleted();

        Enum.TryParse<EntityStatus>(request.Status, true, out var status);

        brandFromDb.Update(
            name: request.Name,
            slug: slug,
            description: request.Description,
            updatedBy: request.UpdatedBy,
            status: status
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Brands", cancellationToken);

        return Result.Success("Update brand successfully.");
    }
}
