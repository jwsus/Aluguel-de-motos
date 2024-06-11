using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Mottu.Application.Orders.Commands;
using Mottu.Application.Orders.Handlers;
using Mottu.Application.Orders.Queries;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Repositories;
using Xunit;

namespace Mottu.Application.Tests.Orders.Handlers
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly Mock<IDeliverymanRepository> _mockDeliverymanRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockDeliverymanRepository = new Mock<IDeliverymanRepository>();
            _mockMediator = new Mock<IMediator>();

            _handler = new CreateOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockDeliverymanRepository.Object,
                _mockNotificationRepository.Object,
                _mockMediator.Object
            );
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldCreateOrderAndSendNotifications()
        {
            // Arrange
            var command = new CreateOrderCommand { Value = 100 };
            var orderId = Guid.NewGuid();
            var eligibleDeliverymen = new List<Deliveryman>
            {
                new Deliveryman { Id = Guid.NewGuid() },
                new Deliveryman { Id = Guid.NewGuid() }
            };

            _mockOrderRepository
                .Setup(repo => repo.AddOrderAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask)
                .Callback<Order>(order => order.Id = orderId);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetEligibleDeliverymanQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(eligibleDeliverymen);

            _mockNotificationRepository
                .Setup(repo => repo.AddNotificationsAsync(It.IsAny<List<Notification>>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockOrderRepository.Verify(repo => repo.AddOrderAsync(It.IsAny<Order>()), Times.Once);
            _mockMediator.Verify(m => m.Send(It.IsAny<GetEligibleDeliverymanQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockNotificationRepository.Verify(repo => repo.AddNotificationsAsync(It.IsAny<List<Notification>>()), Times.Once);
            Assert.Equal(orderId, result);
        }
    }
}
