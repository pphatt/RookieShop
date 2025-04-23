using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name;
        var description = request.Description;
        var parentCategoryId = request.ParentCategoryId;
        var updatedBy = request.UpdatedBy;

        var categoryFromDb = await _categoryRepository.FindByIdAsync(request.Id);

        if (categoryFromDb is null)
            throw new Exceptions.Category.NotFound();

        var duplicateName = _categoryRepository.FindByCondition(x => x.Name.Equals(name)).FirstOrDefault();

        if (duplicateName is not null && duplicateName.Id != categoryFromDb.Id)
            throw new Exceptions.Category.DuplicateName();

        var parentCategory = parentCategoryId is not null
            ? await _categoryRepository.FindByIdAsync((Guid)parentCategoryId)
            : null;

        categoryFromDb.Update(
            name: name,
            description: description,
            parent: parentCategory,
            updatedBy: updatedBy
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Update category successfully.");
    }
}
