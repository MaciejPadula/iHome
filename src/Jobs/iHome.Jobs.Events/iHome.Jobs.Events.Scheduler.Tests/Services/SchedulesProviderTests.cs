using FluentAssertions;
using iHome.Jobs.Events.Infrastructure.Helpers;
using iHome.Jobs.Events.Infrastructure.Models;
using iHome.Jobs.Events.Infrastructure.Repositories;
using iHome.Jobs.Events.Scheduler.Logic;
using iHome.Jobs.Events.Services;
using NSubstitute;

namespace iHome.Jobs.Events.Scheduler.Tests.Services;

public class SchedulesProviderTests
{
    private SchedulesProvider _sut;

    private IScheduleRepository _repository;
    private IScheduleHistoryRepository _historyRepository;
    private IDateTimeProvider _dateTimeProvider;
    private IScheduleRunningConditionChecker _checker;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IScheduleRepository>();
        _historyRepository = Substitute.For<IScheduleHistoryRepository>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _checker = Substitute.For<IScheduleRunningConditionChecker>();

        _sut = new SchedulesProvider(_repository, _historyRepository, _dateTimeProvider, _checker);
    }

    [Test]
    public void GetSchedulesWithDevicesExcluding_WhenExecuted_ShouldReturnExpectedSchedulesToRun()
    {
        //Arrange
        _repository.GetSchedulesWithDevicesExcluding(default!).ReturnsForAnyArgs(new List<Schedule>
        {
            new Schedule
            {
                Id = Guid.Parse("b3106c32-497d-4679-b39f-9dfde172d95b"),
                Name = "Lamp",
                Hour = 21,
                Minute = 37,
                Modified = DateTime.MinValue,
                UserId = "UserXD"
            },
            new Schedule
            {
                Id = Guid.Parse("f5c47eb9-f412-4f88-9c81-3af0d3a6cbae"),
                Name = "Test",
                Hour = 4,
                Minute = 20,
                Modified = DateTime.MinValue,
                UserId = "UserXD"
            }
        });
        _checker.CheckScheduleRunCondition(21, 37).Returns(false);
        _checker.CheckScheduleRunCondition(4, 20).Returns(true);

        var expectedSchedules = new List<Schedule>
        {
            new Schedule
            {
                Id = Guid.Parse("f5c47eb9-f412-4f88-9c81-3af0d3a6cbae"),
                Name = "Test",
                Hour = 4,
                Minute = 20,
                Modified = DateTime.MinValue,
                UserId = "UserXD"
            }
        };

        //Act
        var result = _sut.GetSchedulesToRun();

        //Assert
        result.Should().BeEquivalentTo(expectedSchedules);
    }
}
