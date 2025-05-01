using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;

using Exceptions = Domain.Exceptions.Exceptions;

public class InactivateCategoryCommandHandler : ICommandHandler<InactivateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public InactivateCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(InactivateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FindByIdAsync(request.Id);

        if (category is null)
            throw new Exceptions.Category.NotFound();

        if (category.IsDeleted)
            throw new Exceptions.Category.AlreadyDeleted();

        if (category.Status == EntityStatus.Inactive)
            throw new Exceptions.Category.AlreadyInactivate();

        category.Inactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Inactivate category successfully.");
    }
}
