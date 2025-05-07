using FluentValidation.TestHelper;

using HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.DeleteBrand;

[Trait("Brand", "DeleteBrand")]
public class DeleteBrandCommandValidatorTests : BaseTest
{
    private readonly DeleteBrandCommandValidator _validator;

    public DeleteBrandCommandValidatorTests()
    {
        _validator = new DeleteBrandCommandValidator();
    }

    [Fact]
    public async Task Validator_ShouldNotHaveError_WhenCommandIsValidAsync()
    {
        // Arrange
        var command = new DeleteBrandCommand
        {
            Id = Guid.NewGuid(),
            UpdatedBy = Guid.NewGuid()
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validator_ShouldNotHaveError_WhenFieldsAreEmpty()
    {
        // Arrange
        var command = new DeleteBrandCommand
        {
            Id = Guid.Empty,
            UpdatedBy = Guid.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
