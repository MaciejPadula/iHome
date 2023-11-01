using FluentAssertions;
using iHome.Core.Logic.RoomDtoList;
using iHome.Core.Models;
using iHome.Microservices.RoomsManagement.Contract.Models;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Tests.Logic.RoomDtoList;

public class RoomDtoListBuilderTests
{
    private RoomDtoListBuilder _sut;

    [SetUp]
    public void SetUp()
    {
        var bd = new RoomDtoBuilder();

        _sut = new RoomDtoListBuilder(bd);
    }

    [Test]
    public async Task Build_WhenEmptyDictionaryProvided_ShouldReturnRoomsAndUsersWithIds()
    {
        // Arrange
        var rooms = new List<RoomModel>
        {
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), UserId = "1" },
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), UserId = "2" }
        };
        var users = new Dictionary<string, User>();
        var expectedRooms = new List<RoomDto>
        {
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), User = new(){ Id= "1" } },
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), User = new(){ Id= "2" } }
        };

        // Act
        var result = await _sut.Build(rooms, users);

        // Assert
        result.Should().BeEquivalentTo(expectedRooms);
    }

    [Test]
    public async Task Build_WhenUsersProvided_ShouldReturnRoomsWithUsers()
    {
        // Arrange
        var rooms = new List<RoomModel>
        {
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), UserId = "1" },
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), UserId = "2" }
        };
        var users = new Dictionary<string, User>
        {
            { "1", new() { Id = "1", Name = "name", Email = "test@mail" } },
            { "2", new() { Id = "2", Name = "Maciej", Email = "maciej@wp" } }
        };
        var expectedRooms = new List<RoomDto>
        {
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), User = new() { Id = "1", Name = "name", Email = "test@mail" } },
            new() { Id = Guid.Parse("7c36c4e9-405e-4774-8627-ecc87b7b4754"), User = new() { Id = "2", Name = "Maciej", Email = "maciej@wp" } }
        };

        // Act
        var result = await _sut.Build(rooms, users);

        // Assert
        result.Should().BeEquivalentTo(expectedRooms);
    }
}
