using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;

using Exceptions = Domain.Exceptions.Exceptions;

public class ActivateCategoryCommandHandler : ICommandHandler<ActivateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICacheService _cacheService;

    public ActivateCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(ActivateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FindByIdAsync(request.Id);

        if (category is null)
            throw new Exceptions.Category.NotFound();

        if (category.IsDeleted)
            throw new Exceptions.Category.AlreadyDeleted();

        if (category.Status == EntityStatus.Active)
            throw new Exceptions.Category.AlreadyActive();

        category.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Categories", cancellationToken);

        return Result.Success("Activate category successfully.");
    }
}
