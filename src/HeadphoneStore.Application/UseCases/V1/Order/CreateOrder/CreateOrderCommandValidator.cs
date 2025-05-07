using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
    }
}
