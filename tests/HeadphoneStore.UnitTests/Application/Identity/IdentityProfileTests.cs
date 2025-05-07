using AutoMapper;
using FluentAssertions;
using HeadphoneStore.Application.Mapper;
using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity;

[Trait("Authentication", "Login")]
public class IdentityProfileTests
{
    private readonly IMapper _mapper;

    public IdentityProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfiles>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public async Task CreateMap_ShouldMapCorrectly()
    {
        // Arrange

        // Act

        // Assert
    }
}