using FluentValidation.TestHelper;

using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.CreateBrand;

[Trait("Brand", "CreateBrand")]
public class CreateBrandCommandValidatorTests : BaseTest
{
    private readonly CreateBrandCommandValidator _validator;

    public CreateBrandCommandValidatorTests()
    {
        _validator = new CreateBrandCommandValidator();
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenNameIsNull()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = null,
            Description = "Valid description",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "",
            Description = "Valid description",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = new string('A', 51),
            Description = "Valid description",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("The length of 'Name' must be 50 characters or fewer. You entered 51 characters.");
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenDescriptionIsNull()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "ValidName",
            Description = null,
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("'Description' must not be empty.");
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "ValidName",
            Description = "",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("'Description' must not be empty.");
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "ValidName",
            Slug = "valid-slug",
            Description = "Valid description",
            Status = "Active",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenOptionalFieldsAreNull()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "ValidName",
            Slug = null,
            Description = "Valid description",
            Status = null,
            CreatedBy = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
