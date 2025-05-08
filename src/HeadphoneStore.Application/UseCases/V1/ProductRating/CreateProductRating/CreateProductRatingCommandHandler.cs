using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.ProductRating.CreateProductRating;

using ProductRating = Domain.Aggregates.Products.Entities.ProductRating;
using Exceptions = Domain.Exceptions.Exceptions;

public class CreateProductRatingCommandHandler : ICommandHandler<CreateProductRatingCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductRepository _productRepository;
    private readonly IProductRatingRepository _productRatingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CreateProductRatingCommandHandler(UserManager<AppUser> userManager,
                                             IProductRepository productRepository,
                                             IProductRatingRepository productRatingRepository,
                                             IUnitOfWork unitOfWork,
                                             ICacheService cacheService)
    {
        _userManager = userManager;
        _productRepository = productRepository;
        _productRatingRepository = productRatingRepository;
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

        var productRating = ProductRating.Create(productId: product.Id, customerId: user.Id, ratingValue: request.RatingValue, comment: request.Comment);

        _productRatingRepository.Add(productRating);

        if (product.TotalReviews == 0)
        {
            product.AverageRating = request.RatingValue;
        }
        else
        {
            product.AverageRating = Math.Round(((product.AverageRating * product.TotalReviews) + request.RatingValue) / (product.TotalReviews + 1), 2);
        }

        product.TotalReviews += 1;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Products", cancellationToken);

        return Result.Success("Preview product successfully.");
    }
}
