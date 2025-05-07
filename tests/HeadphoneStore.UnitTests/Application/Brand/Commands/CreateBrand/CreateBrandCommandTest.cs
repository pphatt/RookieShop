using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;
using HeadphoneStore.Shared.Services.Brand.Create;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.CreateBrand;

[Trait("Brand", "CreateBrand")]
public class CreateBrandCommandTests : BaseTest
{
    [Fact]
    public void CreateBrandCommand_ShouldMapCorrectly()
    {
        // Arrange
        var name = "BrandName";
        var slug = "brand-slug";
        var description = "Brand description";
        var status = "Active";
        var requestDto = new CreateBrandRequestDto
        {
            Name = name,
            Slug = slug,
            Description = description,
            Status = status
        };

        // Act
        var command = _mapper.Map<CreateBrandCommand>(requestDto);

        // Assert
        command.Should().NotBeNull();
        command.Name.Should().Be(name);
        command.Slug.Should().Be(slug);
        command.Description.Should().Be(description);
        command.Status.Should().Be(status);
    }
}
