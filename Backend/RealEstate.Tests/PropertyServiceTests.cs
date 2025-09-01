using Moq;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private Mock<IPropertyImageRepository> _mockImageRepo;
        private PropertyService _service;

        [SetUp]
        public void Setup()
        {
            _mockPropertyRepo = new Mock<IPropertyRepository>();
            _mockImageRepo = new Mock<IPropertyImageRepository>();
            _service = new PropertyService(_mockPropertyRepo.Object, _mockImageRepo.Object);
        }

        [Test]
        public async Task CreatePropertyAsync_ShouldReturnPropertyId()
        {
            // Arrange
            var dto = new PropertyDto
            {
                Name = "Casa Test",
                Address = "Calle 123",
                Price = 200000,
                CodeInternal = "INT-001",
                Year = 2020,
                IdOwner = 1
            };

            _mockPropertyRepo
                .Setup(r => r.AddAsync(It.IsAny<Property>()))
                .Callback<Property>(p => p.IdProperty = 99)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreatePropertyAsync(dto);

            // Assert
            Assert.AreEqual(99, result);
            _mockPropertyRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
        }

        [Test]
        public async Task AddImageAsync_ShouldCallRepository()
        {
            // Arrange
            var dto = new PropertyImageDto
            {
                IdProperty = 1,
                File = new byte[] { 0x01, 0x02 },
                Enabled = true
            };

            // Act
            await _service.AddImageAsync(dto);

            // Assert
            _mockImageRepo.Verify(r => r.AddAsync(It.Is<PropertyImage>(i =>
                i.IdProperty == dto.IdProperty &&
                i.File == dto.File &&
                i.Enabled == dto.Enabled
            )), Times.Once);
        }

        [Test]
        public async Task ChangePriceAsync_ShouldUpdatePriceAndAddTrace()
        {
            // Arrange
            var property = new Property { IdProperty = 1, Price = 100000 };

            _mockPropertyRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(property);

            // Act
            await _service.ChangePriceAsync(1, 150000, "Admin");

            // Assert
            Assert.AreEqual(150000, property.Price);
            _mockPropertyRepo.Verify(r => r.AddTraceAsync(It.IsAny<PropertyTrace>()), Times.Once);
            _mockPropertyRepo.Verify(r => r.UpdateAsync(It.Is<Property>(p => p.Price == 150000)), Times.Once);
        }

        [Test]
        public void ChangePriceAsync_ShouldThrowException_WhenPropertyNotFound()
        {
            // Arrange
            _mockPropertyRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Property?)null);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.ChangePriceAsync(99, 150000, "Admin"));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdatePropertyAndAddTrace()
        {
            // Arrange
            var property = new Property
            {
                IdProperty = 1,
                Name = "Casa Vieja",
                Price = 100000
            };

            _mockPropertyRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(property);

            var dto = new PropertyDto
            {
                IdProperty = 1,
                Name = "Casa Nueva",
                Address = "Nueva Direccion",
                Price = 200000,
                CodeInternal = "NUEVO-001",
                Year = 2022,
                IdOwner = 2
            };

            // Act
            await _service.UpdateAsync(dto);

            // Assert
            Assert.AreEqual("Casa Nueva", property.Name);
            Assert.AreEqual(200000, property.Price);
            _mockPropertyRepo.Verify(r => r.UpdateAsync(It.Is<Property>(p => p.Name == "Casa Nueva")), Times.Once);
            _mockPropertyRepo.Verify(r => r.AddTraceAsync(It.IsAny<PropertyTrace>()), Times.Once);
        }

        [Test]
        public async Task GetFilteredAsync_ShouldReturnMatchingProperties()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { IdProperty = 1, Name = "Casa Bonita", Price = 100000 },
                new Property { IdProperty = 2, Name = "Casa Lujosa", Price = 300000 }
            };

            _mockPropertyRepo.Setup(r => r.GetFilteredAsync("Casa", 50000, 200000))
                .ReturnsAsync(properties.Where(p => p.Price >= 50000 && p.Price <= 200000));

            // Act
            var result = await _service.GetFilteredAsync("Casa", 50000, 200000);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Casa Bonita", result.First().Name);
        }
    }
}
