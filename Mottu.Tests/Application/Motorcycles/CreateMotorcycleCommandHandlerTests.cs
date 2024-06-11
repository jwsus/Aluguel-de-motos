using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit; // Para usar Assert e Fact
using Moq; // Para usar Moq e Times
using Mottu.Application.Motorcycles.Commands;
using Mottu.Application.Motorcycles.CreateMotorcycle.Commands;
using Mottu.Application.Interfaces;
using Mottu.Domain.Entities;

namespace Mottu.Tests.Application.Motorcycles
{
    public class CreateMotorcycleCommandHandlerTests
    {
        private readonly Mock<IMotorcycleRepository> _mockRepo;
        private readonly CreateMotorcycleCommandHandler _handler;

        public CreateMotorcycleCommandHandlerTests()
        {
            _mockRepo = new Mock<IMotorcycleRepository>();
            _handler = new CreateMotorcycleCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateMotorcycle()
        {
            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Year = 2023,
                Model = "Model X",
                LicensePlate = "ABC1234"
            };

            _mockRepo.Setup(repo => repo.LicensePlateExistsAsync(command.LicensePlate))
                     .ReturnsAsync(false);

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Motorcycle>()))
                     .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepo.Verify(repo => repo.LicensePlateExistsAsync(command.LicensePlate), Times.Once);
            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Motorcycle>()), Times.Once);
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task Handle_DuplicateLicensePlate_ShouldThrowException()
        {
            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Year = 2023,
                Model = "Model X",
                LicensePlate = "ABC1234"
            };

            _mockRepo.Setup(repo => repo.LicensePlateExistsAsync(command.LicensePlate))
                     .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
            _mockRepo.Verify(repo => repo.LicensePlateExistsAsync(command.LicensePlate), Times.Once);
            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Motorcycle>()), Times.Never);
        }
    }
}