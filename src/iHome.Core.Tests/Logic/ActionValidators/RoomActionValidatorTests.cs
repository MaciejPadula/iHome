using FluentAssertions;
using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Logic.AccessGuards;
using iHome.Core.Logic.ActionValidators;
using iHome.Microservices.UsersApi.Contract;
using NSubstitute;

namespace iHome.Core.Tests.Logic.ActionValidators;

public class RoomActionValidatorTests
{
    [Test]
    public void ValidateReadAccess_WhenUserIsNotOwnerOfRoomAndItIsNotShared_ShouldReturnTrue()
    {
        var roomId = Guid.Parse("d1334250-f240-11ed-a05b-0242ac120003");
        var userId = "xd";

        //Arrange
        var sut = ConfigureSut((users, access) =>
        {
            access.UserHasReadAccess(roomId, userId).Returns(Task.FromResult(false));
        });

        //Act
        var result = sut.ValidateReadAccess(roomId, userId, out var ex);

        //Assert
        result.Should().BeFalse();
        ex.Should().BeOfType(typeof(AccessDeniedException));
    }

    [Test]
    public void ValidateReadAccess_WhenUserIsOwnerOfRoom_ShouldReturnTrue()
    {
        var roomId = Guid.Parse("d1334250-f240-11ed-a05b-0242ac120003");
        var userId = "xd";

        //Arrange
        var sut = ConfigureSut((users, access) =>
        {
            access.UserHasReadAccess(roomId, userId).Returns(Task.FromResult(true));
        });

        //Act
        var result = sut.ValidateReadAccess(roomId, userId, out var ex);

        //Assert
        result.Should().BeTrue();
        ex.Should().BeNull();
    }

    [Test]
    public void ValidateReadAccess_WhenUserIsNotOwnerOfRoomAndItIsShared_ShouldReturnTrue()
    {
        var roomId = Guid.Parse("d1334250-f240-11ed-a05b-0242ac120003");
        var userId = "xd";

        //Arrange
        var sut = ConfigureSut((users, access) =>
        {
            access.UserHasReadAccess(roomId, userId).Returns(Task.FromResult(true));
        });

        //Act
        var result = sut.ValidateReadAccess(roomId, userId, out var ex);

        //Assert
        result.Should().BeTrue();
        ex.Should().BeNull();
    }

    private static RoomActionValidator ConfigureSut(Action<IUserManagementService, IRoomAccessGuard> config)
    {
        var userService = Substitute.For<IUserManagementService>();
        var accessGuard = Substitute.For<IRoomAccessGuard>();

        config.Invoke(userService, accessGuard);

        return new RoomActionValidator(accessGuard, userService);
    }
}
