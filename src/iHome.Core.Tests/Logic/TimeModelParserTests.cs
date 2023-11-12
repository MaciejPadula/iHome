using FluentAssertions;
using iHome.Core.Logic;
using iHome.Model;

namespace iHome.Core.Tests.Logic;

public class TimeModelParserTests
{
    private TimeModelParser _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new TimeModelParser();
    }

    [Test]
    [TestCase("12:20", 12, 20)]
    [TestCase("21:37", 21, 37)]
    [TestCase("0:0", 0, 0)]
    [TestCase("23:59", 23, 59)]
    public void Parse_WhenCorrectStringsProvided_ShouldReturnParsedTimes(string input, int expectedHour, int expectedMinute)
    {
        // Arrange
        var expectedModel = new TimeModel(expectedHour, expectedMinute, true);

        // Act
        var result = _sut.Parse(input);
        
        // Assert
        result.Should().BeEquivalentTo(expectedModel);
    }

    [Test]
    [TestCase("12:2asdasd")]
    [TestCase("2137")]
    [TestCase("12:20:12:1")]
    public void Parse_WhenNotCorrectStringsProvided_ShouldReturnDefaultTime(string input)
    {
        // Arrange
        var expectedModel = new TimeModel(0, 0, false);

        // Act
        var result = _sut.Parse(input);

        // Assert
        result.Should().BeEquivalentTo(expectedModel);
    }
}
