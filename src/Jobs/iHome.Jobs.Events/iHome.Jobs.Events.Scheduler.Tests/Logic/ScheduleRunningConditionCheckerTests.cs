using FluentAssertions;
using iHome.Jobs.Events.Infrastructure.Helpers;
using iHome.Jobs.Events.Scheduler.Logic;
using NSubstitute;

namespace iHome.Jobs.Events.Scheduler.Tests.Logic;

public class ScheduleRunningConditionCheckerTests
{
    private ScheduleRunningConditionChecker _sut;
    private IDateTimeProvider _dateTimeProvider;

    [SetUp]
    public void SetUp()
    {
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        _sut = new ScheduleRunningConditionChecker(_dateTimeProvider);
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(10, 0)]
    [TestCase(11, 59)]
    [TestCase(12, 0)]
    public void CheckScheduleRunCondition_WhenRunTimePassed_ShouldRunSchedule(int hour, int minute)
    {
        //Arrange
        _dateTimeProvider.UtcNow.Returns(new DateTime(2001, 7, 23, 12, 0, 0));

        //Act
        var result = _sut.CheckScheduleRunCondition(hour, minute);

        //Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCase(23, 59)]
    [TestCase(12, 1)]
    public void CheckScheduleRunCondition_WhenRunTimeIsInTheFuture_ShouldNotRunSchedule(int hour, int minute)
    {
        //Arrange
        _dateTimeProvider.UtcNow.Returns(new DateTime(2001, 7, 23, 12, 0, 0));

        //Act
        var result = _sut.CheckScheduleRunCondition(hour, minute);

        //Assert
        result.Should().BeFalse();
    }

    [Test]
    public void CheckScheduleRunCondition_WhenRunTimeIsEquivalentToCurrentTime_ShouldRunSchedule()
    {
        //Arrange
        _dateTimeProvider.UtcNow.Returns(new DateTime(2001, 7, 23, 12, 0, 0));

        //Act
        var result = _sut.CheckScheduleRunCondition(12, 0);

        //Assert
        result.Should().BeTrue();
    }
}
