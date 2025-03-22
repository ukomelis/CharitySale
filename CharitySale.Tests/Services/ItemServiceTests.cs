using AutoFixture;
using AutoMapper;
using CharitySale.Api.Repositories;
using CharitySale.Api.Services;
using CharitySale.Shared.Models;
using Microsoft.Extensions.Logging;
using Moq;
using ItemEntity = CharitySale.Api.Entities.Item;

namespace CharitySale.Tests.Services;

[TestFixture]
public class ItemServiceTests
{
    private Mock<IItemRepository> _mockRepository;
    private Mock<ILogger<ItemService>> _mockLogger;
    private Mock<IMapper> _mockMapper;
    private ItemService _service;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IItemRepository>();
        _mockLogger = new Mock<ILogger<ItemService>>();
        _mockMapper = new Mock<IMapper>();
        _service = new ItemService(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
        _fixture = new Fixture();
        
        // Remove the ThrowingRecursionBehavior and add OmitOnRecursionBehavior
        // This is required because the Item entity has a reference to itself and circular references need to be fixed
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

    }

    [Test]
    public async Task GetItemByIdAsync_WhenItemExists_ReturnsItem()
    {
        // Arrange
        var id = Guid.NewGuid();
        var item = _fixture.Create<ItemEntity>();
        var itemDto = _fixture.Create<Item>();

        _mockRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(item);
        _mockMapper.Setup(x => x.Map<Item>(item))
            .Returns(itemDto);

        // Act
        var result = await _service.GetItemByIdAsync(id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(itemDto));
    }

    [Test]
    public async Task GetItemByIdAsync_WhenItemDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((ItemEntity)null!);

        // Act
        var result = await _service.GetItemByIdAsync(id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Does.Contain(id.ToString()));
    }

    [Test]
    public async Task CreateItemAsync_Success_ReturnsCreatedItem()
    {
        // Arrange
        var createItem = _fixture.Create<CreateItem>();
        var item = _fixture.Create<ItemEntity>();
        var itemDto = _fixture.Create<Item>();

        _mockMapper.Setup(x => x.Map<ItemEntity>(createItem))
            .Returns(item);
        _mockMapper.Setup(x => x.Map<Item>(item))
            .Returns(itemDto);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateItemAsync(createItem);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(itemDto));
        _mockRepository.Verify(x => x.AddAsync(item), Times.Once);
    }

    [TestCase(10, -5, 5)]   // Decrement by 5
    [TestCase(10, 5, 15)]   // Increment by 5
    [TestCase(10, -15, -5)] // Attempt to decrement below 0
    public async Task UpdateItemQuantityInternalAsync_DecrementStock_HandlesQuantityCorrectly(
        int initialQuantity, int change, int expectedQuantity)
    {
        // Arrange
        var id = Guid.NewGuid();
        var item = _fixture.Build<ItemEntity>()
            .With(x => x.Quantity, initialQuantity)
            .Create();
        var itemDto = _fixture.Create<Item>();

        _mockRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(item);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(true);
        _mockMapper.Setup(x => x.Map<Item>(It.IsAny<ItemEntity>()))
            .Returns(itemDto);

        // Act
        var result = await _service.UpdateItemQuantityInternalAsync(id, change, false);

        // Assert
        if (expectedQuantity >= 0)
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(item.Quantity, Is.EqualTo(expectedQuantity));
        }
        else
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Does.Contain("Cannot reduce quantity"));
            });
        }
    }
}
