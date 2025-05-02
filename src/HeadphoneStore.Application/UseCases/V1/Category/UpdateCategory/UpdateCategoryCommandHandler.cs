using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICacheService _cacheService;

    public UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name;
        var description = request.Description;
        var parentCategoryId = request.ParentCategoryId;
        var updatedBy = request.UpdatedBy;

        var categoryFromDb = await _categoryRepository.FindByIdAsync(request.Id, cancellationToken, x => x.SubCategories);

        if (categoryFromDb is null)
            throw new Exceptions.Category.NotFound();

        var duplicateName = _categoryRepository.FindByCondition(x => x.Name.Equals(name)).FirstOrDefault();

        if (duplicateName is not null && duplicateName.Id != categoryFromDb.Id)
            throw new Exceptions.Category.DuplicateName();

        if (categoryFromDb.SubCategories.Count > 0)
            throw new Exceptions.Category.HasSubCategories();

        if (categoryFromDb.Id == parentCategoryId)
            throw new Exceptions.Category.CannotReferenceThemself();

        var slug = categoryFromDb.Name != name ? name.Slugify() : request.Slug;
        var isSlugAlreadyExisted = await _categoryRepository.IsSlugAlreadyExisted(slug, categoryFromDb.Id);

        if (isSlugAlreadyExisted)
            throw new Exceptions.Category.SlugExists();

        if (categoryFromDb.IsDeleted)
            throw new Exceptions.Category.AlreadyDeleted();

        var parentCategory = parentCategoryId is not null
            ? await _categoryRepository.FindByIdAsync((Guid)parentCategoryId)
            : null;

        Enum.TryParse<EntityStatus>(request.Status, true, out var status);

        categoryFromDb.Update(
            name: name,
            description: description,
            parent: parentCategory,
            status: status,
            updatedBy: updatedBy
        );

        categoryFromDb.Slug = slug;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Categories", cancellationToken);

        return Result.Success("Update category successfully.");
    }
}
