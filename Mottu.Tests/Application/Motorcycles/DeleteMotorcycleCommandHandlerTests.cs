using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Interfaces;
using Mottu.Application.Motorcycles.Commands;
using Mottu.Application.Motorcycles.Queries;
using Mottu.Domain.Entities;
using Xunit;

namespace Mottu.Application.Tests.Motorcycles.Commands
{
    public class DeleteMotorcycleCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly DeleteMotorcycleCommandHandler _handler;

        public DeleteMotorcycleCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();
            _mockMediator = new Mock<IMediator>();

            _handler = new DeleteMotorcycleCommandHandler(_mockContext.Object, _mockMotorcycleRepository.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task Handle_GivenNoRentals_DeletesMotorcycle()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var command = new DeleteMotorcycleCommand { Id = motorcycleId };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<CheckMotorcycleHasRentalsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockContext
                .Setup(c => c.Motorcycles.Remove(It.IsAny<Motorcycle>()))
                .Verifiable();

            _mockContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<CheckMotorcycleHasRentalsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(c => c.Motorcycles.Remove(It.Is<Motorcycle>(m => m.Id == command.Id)), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_GivenHasRentals_ThrowsInvalidOperationException()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var command = new DeleteMotorcycleCommand { Id = motorcycleId };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<CheckMotorcycleHasRentalsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));

            _mockMediator.Verify(m => m.Send(It.IsAny<CheckMotorcycleHasRentalsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(c => c.Motorcycles.Remove(It.IsAny<Motorcycle>()), Times.Never);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
