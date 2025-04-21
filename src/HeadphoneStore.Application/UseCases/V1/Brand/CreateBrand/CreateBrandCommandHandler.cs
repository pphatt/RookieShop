using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

using Brand = Domain.Aggregates.Products.Entities.Brand;
using Exceptions = Domain.Exceptions.Exceptions;

public class CreateBrandCommandHandler : ICommandHandler<CreateBrandCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBrandCommandHandler(UserManager<AppUser> userManager, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.CreatedBy.ToString());

        if (user is null)
        {
            throw new Exceptions.User.NotFound();
        }

        var duplicateName = _brandRepository.FindByCondition(x => x.Name == request.Name).FirstOrDefault();

        if (duplicateName is not null)
        {
            throw new Exceptions.Brand.DuplicateName();
        }

        var category = Brand.Create(
            name: request.Name,
            description: request.Description,
            createdBy: request.CreatedBy
        );

        _brandRepository.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
