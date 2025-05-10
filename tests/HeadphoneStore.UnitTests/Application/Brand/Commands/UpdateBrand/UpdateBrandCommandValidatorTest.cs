using FluentValidation.TestHelper;

using HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;
using HeadphoneStore.Domain.Enumerations;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.UpdateBrand;

[Trait("Brand", "UpdateBrand")]
public class UpdateBrandCommandValidatorTests : BaseTest
{
    private readonly UpdateBrandCommandValidator _validator;

    public UpdateBrandCommandValidatorTests()
    {
        _validator = new UpdateBrandCommandValidator();
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenNameIsNullAsync()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = null,
            Description = "Valid description",
            Status = "Active",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = "",
            Description = "Valid description",
            Status = "Active",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = new string('A', 51),
            Description = "Valid description",
            Status = "Active",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("The length of 'Name' must be 50 characters or fewer. You entered 51 characters.");
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenDescriptionIsNullAsync()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = "ValidName",
            Description = null,
            Status = "Active",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("'Description' must not be empty.");
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = "ValidName",
            Description = "",
            Status = "Active",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("'Description' must not be empty.");
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenStatusIsNull()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = "ValidName",
            Description = "Valid description",
            Status = null,
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
            .WithErrorMessage("'Status' must not be empty.");
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenStatusIsEmpty()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = "ValidName",
            Description = "Valid description",
            Status = "",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
            .WithErrorMessage("'Status' must not be empty.");
    }

    [Fact]
    public async Task Validator_ShouldHaveError_WhenStatusIsInvalid()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Name = "ValidName",
            Description = "Valid description",
            Status = "InvalidStatus",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
            .WithErrorMessage($"Invalid Status value. Valid values are: {string.Join(", ", Enum.GetNames(typeof(EntityStatus)))}.");
    }

    [Fact]
    public async Task Validator_ShouldNotHaveError_WhenCommandIsValidAsync()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Id = Guid.NewGuid(),
            Name = "ValidName",
            Slug = "valid-slug",
            Description = "Valid description",
            Status = "Active",
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
