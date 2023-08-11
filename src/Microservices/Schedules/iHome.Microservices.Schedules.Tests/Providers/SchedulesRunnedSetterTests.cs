using FluentAssertions;
using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using iHome.Microservices.Schedules.Logic.Helpers;
using iHome.Microservices.Schedules.Providers;
using NSubstitute;

namespace iHome.Microservices.Schedules.Tests.Providers;

public class SchedulesRunnedSetterTests
{
    private SchedulesRunnedSetter _sut;
    private IClock _clock;
    private IScheduleRunHistoryRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IScheduleRunHistoryRepository>();
        _clock = Substitute.For<IClock>();

        _sut = new SchedulesRunnedSetter(_repository, _clock);
    }

    [Test]
    public async Task SetRunnedProperty_WhenSchedulesProvided_ShouldSetRunnedValues()
    {
        // Arrange
        _clock.UtcNow.Returns(new DateTime(2001, 07, 23, 11, 0, 0));
        var schedules = new List<ScheduleModel>
        {
            new()
            {
                Id = Guid.Parse("7897f7b9-0d86-450c-b8a1-7520d20daa33")
            },
            new()
            {
                Id = Guid.Parse("24f18c78-1dd9-4e4b-b5cd-1eb72d738e5a")
            }
        };
        _repository.ScheduleRunned(Guid.Parse("7897f7b9-0d86-450c-b8a1-7520d20daa33"), new DateTime(2001, 07, 23, 0, 0, 0)).Returns(Task.FromResult(false));
        _repository.ScheduleRunned(Guid.Parse("24f18c78-1dd9-4e4b-b5cd-1eb72d738e5a"), new DateTime(2001, 07, 23, 0, 0, 0)).Returns(Task.FromResult(true));

        var expectedSchedules = new List<ScheduleModel>
        {
            new()
            {
                Id = Guid.Parse("7897f7b9-0d86-450c-b8a1-7520d20daa33"),
                Runned = false
            },
            new()
            {
                Id = Guid.Parse("24f18c78-1dd9-4e4b-b5cd-1eb72d738e5a"),
                Runned = true
            }
        };

        // Act
        var result = await _sut.SetRunnedProperty(schedules);

        // Assert
        result.Should().BeEquivalentTo(expectedSchedules);
    }
}
