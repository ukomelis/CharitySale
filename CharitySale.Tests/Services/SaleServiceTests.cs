using AutoFixture;
using AutoMapper;
using CharitySale.Api.Models;
using CharitySale.Api.Repositories;
using CharitySale.Api.Services;
using CharitySale.Shared.Models;
using Microsoft.Extensions.Logging;
using Moq;
using ItemEntity = CharitySale.Api.Entities.Item;

namespace CharitySale.Tests.Services;

[TestFixture]
public class SaleServiceTests
{
    private Mock<ISaleRepository> _mockSaleRepository;
    private Mock<IItemRepository> _mockItemRepository;
    private Mock<IItemService> _mockItemService;
    private Mock<ILogger<SaleService>> _mockLogger;
    private Mock<IMapper> _mockMapper;
    private SaleService _service;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _mockSaleRepository = new Mock<ISaleRepository>();
        _mockItemRepository = new Mock<IItemRepository>();
        _mockItemService = new Mock<IItemService>();
        _mockLogger = new Mock<ILogger<SaleService>>();
        _mockMapper = new Mock<IMapper>();
        _service = new SaleService(
            _mockSaleRepository.Object,
            _mockItemRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);
        _fixture = new Fixture();
        
        // Remove the ThrowingRecursionBehavior and add OmitOnRecursionBehavior
        // This is required because the Item entity has a reference to itself and circular references need to be fixed
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

    }

    [TestCase(100.50, 50.00, 50.50)]
    [TestCase(20.00, 20.00, 0.00)]
    [TestCase(50.00, 20.00, 30.00)]
    public void CalculateChangeDenominations_ReturnsCorrectChange(
        decimal amountPaid, decimal totalAmount, decimal expectedChange)
    {
        // Arrange
        var amount = amountPaid - totalAmount;

        // Act
        var result = _service.CalculateChangeDenominations(amount);

        // Assert
        Assert.That(result, Is.Not.Null);
        var totalChange = result.Sum(x => x.Value * x.Count);
        Assert.That(totalChange, Is.EqualTo(expectedChange));
    }

    [Test]
    public async Task ValidateItemsAvailabilityAsync_AllItemsAvailable_ReturnsTrue()
    {
        // Arrange
        var items = _fixture.CreateMany<CreateSaleItem>().ToList();
        
        foreach (var item in items)
        {
            _mockItemRepository.Setup(x => x.GetByIdAsync(item.ItemId))
                .ReturnsAsync(new ItemEntity { Id = item.ItemId, Quantity = item.Quantity, Name = _fixture.Create<string>()});
        }

        // Act
        var result = await _service.ValidateItemsAvailabilityAsync(items);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.True);
    }

    [Test]
    public async Task CalculateTotalAsync_ReturnsCorrectTotal()
    {
        // Arrange
        var items = _fixture.CreateMany<CreateSaleItem>(3).ToList();
        decimal expectedTotal = 0;

        foreach (var item in items)
        {
            var price = _fixture.Create<decimal>();
            expectedTotal += price * item.Quantity;
            
            _mockItemService.Setup(x => x.GetItemByIdAsync(item.ItemId))
                .ReturnsAsync(Result<Item>.Success(new Item { Price = price }));
            _mockItemRepository.Setup(x => x.GetByIdAsync(item.ItemId))
                .ReturnsAsync(new ItemEntity { Price = price, Id = item.ItemId, Name = _fixture.Create<string>()});
        }

        // Act
        var total = await _service.CalculateTotalAsync(items);

        // Assert
        Assert.That(total, Is.EqualTo(expectedTotal));
    }
}
