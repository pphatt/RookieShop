using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.User;
using HeadphoneStore.Shared.Dtos.Order;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.Shared.Services.Order.CreateOrder;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Order.GetOrderById;

using Exceptions = Domain.Exceptions.Exceptions;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(UserManager<AppUser> userManager, IOrderRepository orderRepository)
    {
        _userManager = userManager;
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.CustomerId.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var order = await _orderRepository.GetOrderById(request.Id);

        if (order is null || user.Id != order.Customer.Id)
            throw new Exceptions.Order.NotFound();

        var result = new OrderDto
        {
            Id = order.Id,
            Note = order.Note,
            PhoneNumber = order.PhoneNumber,
            Status = order.Status.ToString(),
            IsFeedback = order.IsFeedback,
            TotalPrice = order.TotalPrice,
            PaymentMethod = order.PaymentMethod.ToString(),
            ShippingAddress = new ShippingAddressDto
            {
                StreetAddress = order.ShippingAddress.StreetAddress,
                Ward = order.ShippingAddress.Ward,
                District = order.ShippingAddress.District,
                CityProvince = order.ShippingAddress.CityProvince
            },
            Customer = new UserDto
            {
                FirstName = order.Customer.FirstName,
                LastName = order.Customer.LastName,
                Email = order.Customer.Email,
                Avatar = order.Customer.Avatar,
            },
            OrderDetails = order.OrderDetails.Select(x => new OrderDetailDto
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
            CreatedDateTime = order.CreatedDateTime,
            ModifiedDateTime = order.ModifiedDateTime
        };

        return Result.Success(result);
    }
}
