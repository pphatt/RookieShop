using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.User;
using HeadphoneStore.Shared.Dtos.Order;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.Shared.Services.Order.CreateOrder;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Order.GetAllOrdersPagination;

using Exceptions = Domain.Exceptions.Exceptions;

public class GetAllOrdersPaginationQueryHandler : IQueryHandler<GetAllOrdersPaginationQuery, PagedResult<OrderDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersPaginationQueryHandler(UserManager<AppUser> userManager, IOrderRepository orderRepository)
    {
        _userManager = userManager;
        _orderRepository = orderRepository;
    }

    public async Task<Result<PagedResult<OrderDto>>> Handle(GetAllOrdersPaginationQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var role = await _userManager.GetRolesAsync(user);

        var isAdmin = role.Contains("admin");

        Guid? userId = isAdmin ? null : user.Id;

        var query = await _orderRepository.GetAllOrdersPagination(
            userId: userId,
            searchTerm: request.SearchTerm,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        var resultItem = query.Items.Select(x => new OrderDto
        {
            Id = x.Id,
            Note = x.Note,
            PhoneNumber = x.PhoneNumber,
            Status = x.Status.ToString(),
            IsFeedback = x.IsFeedback,
            TotalPrice = x.TotalPrice,
            PaymentMethod = x.PaymentMethod.ToString(),
            ShippingAddress = new ShippingAddressDto
            {
                StreetAddress = x.ShippingAddress.StreetAddress,
                Ward = x.ShippingAddress.Ward,
                District = x.ShippingAddress.District,
                CityProvince = x.ShippingAddress.CityProvince
            },
            Customer = new UserDto
            {
                FirstName = x.Customer.FirstName,
                LastName = x.Customer.LastName,
                Email = x.Customer.Email,
                Avatar = x.Customer.Avatar,
            },
            OrderDetails = x.OrderDetails.Select(x => new OrderDetailDto
            {
                Product = new ProductDto
                {
                    Name = x.Product.Name,
                    Slug = x.Product.Slug,
                    ProductPrice = x.Product.ProductPrice.Amount,
                    Media = x.Product.Media.OrderBy(x => x.DisplayOrder).Select(x => new ProductMediaDto
                    {
                        Id = x.Id,
                        ImageUrl = x.ImageUrl,
                        Name = x.Name,
                        Path = x.Path,
                        PublicId = x.PublicId,
                        DIsplayOrder = x.DisplayOrder
                    }).ToList().AsReadOnly(),
                },
                Quantity = x.Quantity
            }).ToList(),
            CreatedDateTime = x.CreatedDateTime,
            ModifiedDateTime = x.ModifiedDateTime
        }).ToList();

        return Result.Success(new PagedResult<OrderDto>(resultItem, query.PageIndex, query.PageSize, query.TotalCount));
    }
}
