﻿using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;

using Category = Domain.Aggregates.Categories.Entities.Category;
using Exceptions = Domain.Exceptions.Exceptions;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICacheService _cacheService;

    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name;
        var description = request.Description;
        var parentCategoryId = request.ParentCategoryId;

        var duplicateName = _categoryRepository.FindByCondition(x => x.Name.Equals(name)).SingleOrDefault();

        if (duplicateName is not null)
            throw new Exceptions.Category.DuplicateName();

        var isSlugAlreadyExisted = await _categoryRepository.IsSlugAlreadyExisted(request.Slug!);

        if (isSlugAlreadyExisted)
            throw new Exceptions.Category.SlugExists();

        var parentCategory = parentCategoryId is not null
            ? await _categoryRepository.FindByIdAsync((Guid)parentCategoryId)
            : null;

        Enum.TryParse<EntityStatus>(request.Status, true, out var status);

        var category = Category.Create(
            name: name,
            slug: request.Slug!,
            description: description,
            parent: parentCategory,
            status: status
        );

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Categories", cancellationToken);

        return Result.Success("Create category successfully.");
    }
}
