using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProductRating;

using Exceptions = Domain.Exceptions.Exceptions;
using ProductRating = Domain.Aggregates.Products.Entities.ProductRating;

public class CreateProductRatingCommandHandler : ICommandHandler<CreateProductRatingCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CreateProductRatingCommandHandler(UserManager<AppUser> userManager,
                                             IProductRepository productRepository,
                                             IUnitOfWork unitOfWork,
                                             ICacheService cacheService)
    {
        _userManager = userManager;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(CreateProductRatingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.CustomerId.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var product = await _productRepository
            .GetQueryableSet()
            .Include(x => x.Ratings)
            .FirstOrDefaultAsync(x => x.Id == request.ProductId);

        if (product is null)
            throw new Exceptions.Product.NotFound();

        var rating = ProductRating.Create(productId: product.Id, customerId: user.Id, ratingValue: request.RatingValue, comment: request.Comment);
        double averageRating = request.RatingValue;
        var totalReviews = product.TotalReviews + 1;

        if (product.TotalReviews != 0)
        {
            averageRating = Math.Round((product.AverageRating * product.TotalReviews + request.RatingValue) / (product.TotalReviews + 1), 2);
        }

        product.AddRating(rating, averageRating, totalReviews);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Products", cancellationToken);

        return Result.Success("Preview product successfully.");
    }
}
