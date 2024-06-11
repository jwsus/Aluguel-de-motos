using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using MediatR;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Orders.Commands;
using Mottu.Application.Orders.Queries;
using Mottu.Application.Deliverymen.Queries;
using Mottu.Domain.Entities;

namespace Mottu.Tests.Application.Orders
{
    public class CompleteOrderCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IMediator> _mockMediator;
        private readonly CompleteOrderCommandHandler _handler;

        public CompleteOrderCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockMediator = new Mock<IMediator>();
            _handler = new CompleteOrderCommandHandler(_mockContext.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCompleteOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var deliverymanId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Situation = OrderSituation.Accepted,
                DeliverymanId = deliverymanId
            };
            var command = new CompleteOrderCommand(orderId, userId.ToString());

            _mockMediator.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(order);
            _mockMediator.Setup(m => m.Send(It.IsAny<GetDeliverymanIdByUserIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(deliverymanId);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.OrderId.Should().Be(orderId);
            result.Status.Should().Be(OrderSituation.Delivered);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidUserId_ShouldThrowException()
        {
            // Arrange
            var command = new CompleteOrderCommand(Guid.NewGuid(), "invalid-guid");

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Invalid user identity.");
        }

        [Fact]
        public async Task Handle_OrderNotFound_ShouldThrowException()
        {
            // Arrange
            var command = new CompleteOrderCommand(Guid.NewGuid(), Guid.NewGuid().ToString());

            _mockMediator.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Order)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Order not found.");
        }

        [Fact]
        public async Task Handle_OrderNotAccepted_ShouldThrowException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Situation = OrderSituation.Available, // Order is not accepted
                DeliverymanId = Guid.NewGuid()
            };
            var command = new CompleteOrderCommand(orderId,userId.ToString())
            ;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(order);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Only accepted orders can be completed.");
        }

        [Fact]
        public async Task Handle_DeliverymanNotFound_ShouldThrowException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Situation = OrderSituation.Accepted,
                DeliverymanId = Guid.NewGuid()
            };
            var command = new CompleteOrderCommand(orderId, userId.ToString());

            _mockMediator.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(order);
            _mockMediator.Setup(m => m.Send(It.IsAny<GetDeliverymanIdByUserIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Guid?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Delivery person not found.");
        }

        [Fact]
        public async Task Handle_DeliverymanDoesNotMatchOrder_ShouldThrowException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Situation = OrderSituation.Accepted,
                DeliverymanId = Guid.NewGuid() // Different deliveryman
            };
            var command = new CompleteOrderCommand(orderId, userId.ToString());


            _mockMediator.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(order);
            _mockMediator.Setup(m => m.Send(It.IsAny<GetDeliverymanIdByUserIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Guid.NewGuid()); // Different deliveryman

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Delivery person does not match the order");
        }
    }
}
