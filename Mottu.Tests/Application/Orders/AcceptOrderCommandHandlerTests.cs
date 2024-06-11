// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Xunit;
// using Moq;
// using FluentAssertions;
// using MediatR;
// using Microsoft.AspNetCore.Http;
// using Mottu.Application.Common.Interfaces;
// using Mottu.Application.Orders.Commands;
// using Mottu.Domain.Entities;
// using Mottu.Application.Deliverymen.Queries;

// namespace Mottu.Tests.Application.Orders
// {
//     public class AcceptOrderCommandHandlerTests
//     {
//         private readonly Mock<IApplicationDbContext> _mockContext;
//         private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
//         private readonly Mock<IMediator> _mockMediator;
//         private readonly AcceptOrderCommandHandler _handler;

//         public AcceptOrderCommandHandlerTests()
//         {
//             _mockContext = new Mock<IApplicationDbContext>();
//             _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
//             _mockMediator = new Mock<IMediator>();
//             _handler = new AcceptOrderCommandHandler(_mockContext.Object, _mockHttpContextAccessor.Object, _mockMediator.Object);
//         }

//         [Fact]
//         public async Task Handle_ValidRequest_ShouldAcceptOrder()
//         {
//             // Arrange
//             var orderId = Guid.NewGuid();
//             var userId = Guid.NewGuid();
//             var deliverymanId = Guid.NewGuid();
//             var order = new Order
//             {
//                 Id = orderId,
//                 Situation = OrderSituation.Available
//             };
//             var command = new AcceptOrderCommand(orderId, userId.ToString())

//             _mockContext.Setup(c => c.Orders.FindAsync(new object[] { orderId }, It.IsAny<CancellationToken>()))
//                         .ReturnsAsync(order);
//             _mockMediator.Setup(m => m.Send(It.IsAny<GetDeliverymanIdByUserIdQuery>(), It.IsAny<CancellationToken>()))
//                          .ReturnsAsync(deliverymanId);
//             _mockMediator.Setup(m => m.Send(It.IsAny<CheckDeliverymanEligibledOrdersQuery>(), It.IsAny<CancellationToken>()))
//                          .ReturnsAsync(true);
//             _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
//                         .Returns(Task.CompletedTask);

//             // Act
//             var result = await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             result.Should().NotBeNull();
//             result.OrderId.Should().Be(orderId);
//             result.Situation.Should().Be(OrderSituation.Accepted);
//             result.DeliverymanId.Should().Be(deliverymanId);
//             _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//         }

//         [Fact]
//         public async Task Handle_InvalidUserId_ShouldThrowException()
//         {
//             // Arrange
//             var command = new AcceptOrderCommand
//             {
//                 OrderId = Guid.NewGuid(),
//                 UserId = "invalid-guid"
//             };

//             // Act
//             Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             await act.Should().ThrowAsync<Exception>().WithMessage("Invalid user identity.");
//         }

//         [Fact]
//         public async Task Handle_OrderNotAvailable_ShouldThrowException()
//         {
//             // Arrange
//             var orderId = Guid.NewGuid();
//             var userId = Guid.NewGuid();
//             var order = new Order
//             {
//                 Id = orderId,
//                 Situation = OrderSituation.Accepted // Order is not available
//             };
//             var command = new AcceptOrderCommand
//             {
//                 OrderId = orderId,
//                 UserId = userId.ToString()
//             };

//             _mockContext.Setup(c => c.Orders.FindAsync(new object[] { orderId }, It.IsAny<CancellationToken>()))
//                         .ReturnsAsync(order);

//             // Act
//             Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             await act.Should().ThrowAsync<Exception>().WithMessage("Invalid or already accepted order.");
//         }

//         [Fact]
//         public async Task Handle_DeliverymanNotFound_ShouldThrowException()
//         {
//             // Arrange
//             var orderId = Guid.NewGuid();
//             var userId = Guid.NewGuid();
//             var order = new Order
//             {
//                 Id = orderId,
//                 Situation = OrderSituation.Available
//             };
//             var command = new AcceptOrderCommand
//             {
//                 OrderId = orderId,
//                 UserId = userId.ToString()
//             };

//             _mockContext.Setup(c => c.Orders.FindAsync(new object[] { orderId }, It.IsAny<CancellationToken>()))
//                         .ReturnsAsync(order);
//             _mockMediator.Setup(m => m.Send(It.IsAny<GetDeliverymanIdByUserIdQuery>(), It.IsAny<CancellationToken>()))
//                          .ReturnsAsync((Guid?)null);

//             // Act
//             Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             await act.Should().ThrowAsync<Exception>().WithMessage("Delivery person not found.");
//         }

//         [Fact]
//         public async Task Handle_DeliverymanNotEligible_ShouldThrowException()
//         {
//             // Arrange
//             var orderId = Guid.NewGuid();
//             var userId = Guid.NewGuid();
//             var deliverymanId = Guid.NewGuid();
//             var order = new Order
//             {
//                 Id = orderId,
//                 Situation = OrderSituation.Available
//             };
//             var command = new AcceptOrderCommand
//             {
//                 OrderId = orderId,
//                 UserId = userId.ToString()
//             };

//             _mockContext.Setup(c => c.Orders.FindAsync(new object[] { orderId }, It.IsAny<CancellationToken>()))
//                         .ReturnsAsync(order);
//             _mockMediator.Setup(m => m.Send(It.IsAny<GetDeliverymanIdByUserIdQuery>(), It.IsAny<CancellationToken>()))
//                          .ReturnsAsync(deliverymanId);
//             _mockMediator.Setup(m => m.Send(It.IsAny<CheckDeliverymanEligibledOrdersQuery>(), It.IsAny<CancellationToken>()))
//                          .ReturnsAsync(false);

//             // Act
//             Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             await act.Should().ThrowAsync<Exception>().WithMessage("Delivery person is not eligible.");
//         }
//     }
// }
