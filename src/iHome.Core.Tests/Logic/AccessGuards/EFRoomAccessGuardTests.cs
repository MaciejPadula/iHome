using FluentAssertions;
using iHome.Core.Logic.AccessGuards;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.ConnectionTables;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace iHome.Core.Tests.Logic.AccessGuards;

public class EFRoomAccessGuardTests
{
    [Test]
    public void UserHasReadAccess_WhenUserIsNotOwnerAndDontHaveRoomShared_ShouldReturnFalse()
    {
        //Arrange
        var sut = ConfigureSut(mock =>
        {
            mock.Rooms.Returns(new List<Room>
            {
                new Room
                {
                    Id = Guid.Empty,
                    Name = "testRoom",
                    UserId = "test"
                }
            }.AsQueryable());

            mock.UserRoom.Returns(new List<UserRoom>
            {
                new UserRoom
                {
                    Id = Guid.NewGuid(),
                    RoomId = Guid.Empty,
                    UserId = "test2"
                }
            }.AsQueryable());
        });

        var userId = "test3";

        //Act
        var result = sut.UserHasReadAccess(Guid.Empty, userId);

        //Assert
        result.Should().Be(false);
    }

    [Test]
    public async Task UserHasReadAccess_WhenUserIsOwner_ShouldReturnTrue()
    {
        //Arrange
        var rooms = GetQueryableFromList(new List<Room>
        {
            new Room
            {
                Id = Guid.Empty,
                Name = "testRoom",
                UserId = "test"
            }
        });
        var userRooms = GetQueryableFromList(new List<UserRoom>
        {
            new UserRoom
            {
                Id = Guid.NewGuid(),
                RoomId = Guid.Empty,
                UserId = "test2"
            }
        });
        var sut = ConfigureSut(mock =>
        {
            mock.Rooms.Returns(rooms);

            mock.UserRoom.Returns(userRooms);
        });

        var userId = "test";

        //Act
        var result = await sut.UserHasReadAccess(Guid.Empty, userId);

        //Assert
        result.Should().Be(true);
    }

    private static EFRoomAccessGuard ConfigureSut(Action<SqlDataContext> configure)
    {
        var context = Substitute.For<SqlDataContext>(new DbContextOptions<SqlDataContext>());
        configure.Invoke(context);

        return new EFRoomAccessGuard(context);
    }

    private static DbSet<T> GetQueryableFromList<T>(List<T> data)
        where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = Substitute.For<DbSet<T>, IQueryable<T>>();

        // Query the set
        ((IQueryable<T>)mockSet.AsAsyncEnumerable()).Provider.Returns(queryable.Provider);
        ((IQueryable<T>)mockSet.AsAsyncEnumerable()).Expression.Returns(queryable.Expression);
        ((IQueryable<T>)mockSet.AsAsyncEnumerable()).ElementType.Returns(queryable.ElementType);
        ((IQueryable<T>)mockSet.AsAsyncEnumerable()).GetEnumerator().Returns(queryable.GetEnumerator());

        mockSet.When(set => set.Add(Arg.Any<T>())).Do(info => data.Add(info.Arg<T>()));
        mockSet.When(set => set.Remove(Arg.Any<T>())).Do(info => data.Remove(info.Arg<T>()));

        return mockSet;
    }
}
