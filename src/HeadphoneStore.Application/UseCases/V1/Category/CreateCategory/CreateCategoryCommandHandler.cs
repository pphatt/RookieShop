using System.Linq;

using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;

using Category = Domain.Aggregates.Categories.Entities.Category;
using Exceptions = Domain.Exceptions.Exceptions;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserManager<AppUser> _userManager;

    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
    }

    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name;
        var description = request.Description;
        var createdBy = request.CreatedBy;
        var parentCategoryId = request.ParentCategoryId;

        var duplicateName = _categoryRepository.FindByCondition(x => x.Name.Equals(name)).SingleOrDefault();

        if (duplicateName is not null)
        {
            throw new Exceptions.Category.DuplicateName();
        }

        var user = await _userManager.FindByIdAsync(createdBy.ToString());

        if (user is null)
        {
            throw new UsersException.NotFound();
        }

        var parentCategory = parentCategoryId is not null 
            ? await _categoryRepository.FindByIdAsync((Guid)parentCategoryId) 
            : null;

        var category = Category.Create(
            name: name,
            description: description,
            createdBy: createdBy,
            parent: parentCategory
        );

        _categoryRepository.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
