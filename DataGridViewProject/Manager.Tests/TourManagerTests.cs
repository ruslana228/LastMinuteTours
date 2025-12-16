using Ahatornn.TestGenerator;
using Entities.Models;
using FluentAssertions;
using Manager.Contracts;
using MemoryStorage.Contracts;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Manager.Tests
{
    /// <summary>
    /// Набор модульных тестов для проверки работы класса <see cref="TourManager"/>
    /// </summary>
    public class TourManagerTests
    {
        private readonly ITourManager tourManager;
        private readonly Mock<ITourStorage> storageMock;
        private readonly Mock<ILogger<TourManager>> loggerMock;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        /// <summary>
        /// Инициализирует экземпляр <see cref="TourManagerTests"/>
        /// </summary>
        public TourManagerTests()
        {
            storageMock = new Mock<ITourStorage>();
            loggerMock = new Mock<ILogger<TourManager>>();
            tourManager = new TourManager(storageMock.Object, loggerMock.Object);
        }

        /// <summary>
        /// Проверяет, что метод GetAll возвращает все туры и вызывает хранилище один раз
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnAllTours()
        {
            // Arrange
            var tour1 = TestEntityProvider.Shared.Create<TourModel>();
            var tour2 = TestEntityProvider.Shared.Create<TourModel>();
            var expectedTours = new List<TourModel> { tour1, tour2 };

            storageMock.Setup(x => x.GetAll(cancellationToken))
                .ReturnsAsync(expectedTours);

            // Act
            var result = await tourManager.GetAll(cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedTours);
            storageMock.Verify(x => x.GetAll(cancellationToken), Times.Once);
        }

        /// <summary>
        /// Проверяет, что метод GetById возвращает правильный тур по Id
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnTour()
        {
            // Arrange
            var expectedTour = TestEntityProvider.Shared.Create<TourModel>();
            storageMock.Setup(x => x.GetById(expectedTour.Id, cancellationToken))
                .ReturnsAsync(expectedTour);

            // Act
            var result = await tourManager.GetById(expectedTour.Id, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedTour.Id);
            storageMock.Verify(x => x.GetById(expectedTour.Id, cancellationToken), Times.Once);
        }

        /// <summary>
        /// Проверяет, что метод Add вызывает хранилище для добавления тура
        /// </summary>
        [Fact]
        public async Task AddShouldCallStorageAdd()
        {
            // Arrange
            var tour = TestEntityProvider.Shared.Create<TourModel>();

            // Act
            await tourManager.Add(tour, cancellationToken);

            // Assert
            storageMock.Verify(x => x.Add(tour, cancellationToken), Times.Once);
        }

        /// <summary>
        /// Проверяет, что метод Update вызывает хранилище для обновления тура
        /// </summary>
        [Fact]
        public async Task UpdateShouldCallStorageUpdate()
        {
            // Arrange
            var tour = TestEntityProvider.Shared.Create<TourModel>();

            // Act
            await tourManager.Update(tour, cancellationToken);

            // Assert
            storageMock.Verify(x => x.Update(tour, cancellationToken), Times.Once);
        }

        /// <summary>
        /// Проверяет, что метод Delete вызывает хранилище для удаления тура по Id
        /// </summary>
        [Fact]
        public async Task DeleteShouldCallStorageDelete()
        {
            // Arrange
            var tourId = Guid.NewGuid();

            // Act
            await tourManager.Delete(tourId, cancellationToken);

            // Assert
            storageMock.Verify(x => x.Delete(tourId, cancellationToken), Times.Once);
        }

        /// <summary>
        /// Проверяет, что метод GetStatistics корректно считает статистику по турам
        /// </summary>
        [Fact]
        public async Task GetStatisticsShouldCalculateCorrectStatistics()
        {
            // Arrange
            var tour1 = TestEntityProvider.Shared.Create<TourModel>(x =>
            {
                x.CostPerVacationer = 10000m;
                x.NumberVacationers = 2;
                x.Surcharges = 500m;
            });

            var tour2 = TestEntityProvider.Shared.Create<TourModel>(x =>
            {
                x.CostPerVacationer = 20000m;
                x.NumberVacationers = 1;
                x.Surcharges = 1000m;
            });

            var tours = new List<TourModel> { tour1, tour2 }.AsReadOnly();
            storageMock.Setup(x => x.GetAll(cancellationToken)).ReturnsAsync(tours);

            // Act
            var result = await tourManager.GetStatistics(cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.TotalToursCount.Should().Be(2);
            result.TotalCostAllTours.Should().Be(10000m * 2 + 500m + 20000m * 1 + 1000m);
            result.ToursWithSurchargesCount.Should().Be(2);
            result.TotalSurcharges.Should().Be(1500m);

            storageMock.Verify(x => x.GetAll(cancellationToken), Times.Once);
        }

        /// <summary>
        /// Проверяет, что методы TourManager логируют время выполнения
        /// </summary>
        [Fact]
        public async Task GetAllShouldLogExecutionTime()
        {
            // Arrange
            var tours = new List<TourModel>().AsReadOnly();
            storageMock.Setup(x => x.GetAll(cancellationToken)).ReturnsAsync(tours);

            // Act
            await tourManager.GetAll(cancellationToken);

            // Assert
            // Проверяем, что был вызов LogDebug с сообщением о времени выполнения
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("TourManager.GetAll выполнен за")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}