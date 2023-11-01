using FluentAssertions;
using iHome.Infrastructure.Cache.Logic;
using iHome.Infrastructure.Cache.Tests.Mocks;

namespace iHome.Infrastructure.Cache.Tests.Logic;

public class CacheFetcherTests
{
    private CacheFetcher _sut;
    private ICache _cache;

    [SetUp]
    public void SetUp()
    {
        _cache = new CacheMock();
        _sut = new(_cache);
    }

    [Test]
    public void FetchCachedByKey_WhenRecordExistsInCache_ShouldReturnElementFromCache()
    {
        // Arrange
        var methodName = "TestName";
        _cache.Set(methodName, new List<TestEntity>
        {
            new(){ Id = 1 },
            new(){ Id = 2 }
        });

        var expectedResult = new TestEntity { Id = 1 };

        // Act
        var result = _sut.FetchCachedByKey<int, TestEntity>(
            1,
            null, // when fetching method is called test will throw exception
            e => e.Id,
            methodName);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void FetchCachedByKey_WhenRecordDoNotExistsInCache_ShouldGetElementFromProvidedSourceAndAddToCache()
    {
        // Arrange
        var methodName = "TestName";
        var database = new List<TestEntity>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };
        _cache.Set(methodName, new List<TestEntity>
        {
            new(){ Id = 1 },
            new(){ Id = 2 }
        });

        var expectedResult = new TestEntity
        {
            Id = 3
        };
        var expectedCache = new List<TestEntity>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };

        // Act
        var result = _sut.FetchCachedByKey(
            3,
            key => database.FirstOrDefault(e => e.Id == key),
            e => e.Id,
            methodName);
        var cache = _cache.Get<List<TestEntity>>(methodName);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        cache.Should().BeEquivalentTo(expectedCache);
    }

    [Test]
    public void FetchCachedByKeys_WhenRecordExistsInCache_ShouldReturnElementFromCache()
    {
        // Arrange
        var methodName = "TestName";
        _cache.Set(methodName, new List<TestEntity>
        {
            new(){ Id = 1 },
            new(){ Id = 2 }
        });

        var expectedResult = new List<TestEntity> { new() { Id = 1 } };

        // Act
        var result = _sut.FetchCachedByKeys<int, TestEntity>(
            new List<int> { 1 },
            null, // when fetching method is called test will throw exception
            e => e.Id,
            methodName);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void FetchCachedByKeys_WhenRecordDoNotExistsInCache_ShouldGetElementFromProvidedSourceAndAddToCache()
    {
        // Arrange
        var methodName = "TestName";
        var database = new List<TestEntity>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };
        _cache.Set(methodName, new List<TestEntity>
        {
            new(){ Id = 1 },
            new(){ Id = 2 }
        });

        var expectedResult = new List<TestEntity>
        {
            new() { Id = 2 },
            new() { Id = 3 }
        };
        var expectedCache = new List<TestEntity>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };

        // Act
        var result = _sut.FetchCachedByKeys(
            new List<int> { 2, 3 },
            keys => database.Where(e => keys.Contains(e.Id)),
            e => e.Id,
            methodName);
        var cache = _cache.Get<List<TestEntity>>(methodName);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        cache.Should().BeEquivalentTo(expectedCache);
    }

    public class TestEntity
    {
        public int Id { get; set; }
    }
}
