using FluentAssertions;
using iHome.Core.Logic.RoomDtoList;
using iHome.Core.Models;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Tests.Logic.RoomDtoList;

public class RoomDtoBuilderTests
{
    private RoomDtoBuilder _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new RoomDtoBuilder();
    }

    [Test]
    [TestCase(default)]
    [TestCase(null)]
    public async Task Build_WhenUserIsDefaultOrNull_ShouldCreateUserWithRoomUserId(User? user)
    {
        // Arrange
        var id = Guid.NewGuid();
        var room = new RoomModel
        {
            Id = id,
            Name = "test",
            UserId = "user1"
        };

        var expectedRoom = new RoomDto
        {
            Id = id,
            Name = "test",
            User = new()
            {
                Id = "user1"
            }
        };

        // Act
        var result = await _sut.Build(room, user);

        // Assert
        result.Should().BeEquivalentTo(expectedRoom);
    }

    [Test]
    public async Task Build_WhenUserProvided_ReturnRoomWithUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        var room = new RoomModel
        {
            Id = id,
            Name = "test",
            UserId = "user1"
        };

        var user = new User
        {
            Id = "user1",
            Name = "test",
            Email = "test@gmail.com"
        };

        var expectedRoom = new RoomDto
        {
            Id = id,
            Name = "test",
            User = new User
            {
                Id = "user1",
                Name = "test",
                Email = "test@gmail.com"
            }
        };

        // Act
        var result = await _sut.Build(room, user);

        // Assert
        result.Should().BeEquivalentTo(expectedRoom);
    }
}
