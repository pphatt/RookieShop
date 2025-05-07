using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;
using HeadphoneStore.Shared.Services.Brand.Update;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.UpdateBrand;


[Trait("Brand", "UpdateBrand")]
public class UpdateBrandCommandTests : BaseTest
{
    [Fact]
    public void UpdateBrandCommand_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "BrandName";
        var slug = "brand-slug";
        var description = "Brand description";
        var status = "Active";
        var requestDto = new UpdateBrandRequestDto
        {
            Id = id,
            Name = name,
            Slug = slug,
            Description = description,
            Status = status
        };

        // Act
        var command = _mapper.Map<UpdateBrandCommand>(requestDto);

        // Assert
        command.Should().NotBeNull();
        command.Id.Should().Be(id);
        command.Name.Should().Be(name);
        command.Slug.Should().Be(slug);
        command.Description.Should().Be(description);
        command.Status.Should().Be(status);
    }
}
