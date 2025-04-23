using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateBrandCommandHandler : ICommandHandler<UpdateBrandCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBrandCommandHandler(
        UserManager<AppUser> userManager,
        IBrandRepository brandRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UpdatedBy.ToString());

        if (user is null)
            throw new Exceptions.User.NotFound();

        var brandFromDb = await _brandRepository.FindByIdAsync(request.Id);

        if (brandFromDb is null)
            throw new Exceptions.Brand.NotFound();

        var duplicateName = _brandRepository.FindByCondition(x => x.Name.Equals(request.Name)).FirstOrDefault();

        if (duplicateName is not null && duplicateName.Id != brandFromDb.Id)
            throw new Exceptions.Brand.DuplicateName();

        brandFromDb.Update(
            name: request.Name,
            description: request.Description,
            updatedBy: request.UpdatedBy
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
