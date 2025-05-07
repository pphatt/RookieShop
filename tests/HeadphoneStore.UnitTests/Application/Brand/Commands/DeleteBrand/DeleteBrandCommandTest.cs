using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.DeleteBrand;
[Trait("Brand", "DeleteBrand")]
public class DeleteBrandCommandTests : BaseTest
{
    [Fact]
    public void DeleteBrandCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();

        // Act
        var command = new DeleteBrandCommand
        {
            Id = id,
            UpdatedBy = updatedBy
        };

        // Assert
        command.Should().NotBeNull();
        command.Id.Should().Be(id);
        command.UpdatedBy.Should().Be(updatedBy);
    }
}
