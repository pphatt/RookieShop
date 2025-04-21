using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;

public class BulkDeleteBrandCommandValidator : AbstractValidator<BulkDeleteBrandCommand>
{
    public BulkDeleteBrandCommandValidator()
    {
    }
}
