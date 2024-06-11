using MediatR;
using Microsoft.Extensions.DependencyInjection;
// using Mottu.Api.Services;

// using Mottu.Api.Services;

// using Mottu.Api.Services;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Deliverymen.Commands;
using Mottu.Application.Interfaces;
using Mottu.Application.Motorcycles.Commands;
using Mottu.Application.Motorcycles.Queries;
using Mottu.Application.Orders.Handlers;
using Mottu.Application.Services;
using Mottu.Infrastructure.Repositories;

namespace Mottu.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registro genérico dos handlers do MediatR
            services.AddMediatR(typeof(CreateMotorcycleCommandHandler).Assembly);
            services.AddMediatR(typeof(GetMotorcycleByIdQueryHandler).Assembly);
            services.AddMediatR(typeof(RegisterDeliverymanCommandHandler).Assembly);
            services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);

            // Registro dos repositórios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeliverymanRepository, DeliverymanRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();

            // Registro dos Serviços
            services.AddSingleton<S3FileService>();
            // services.AddSingleton<OrderNotificationProducer>(); 
            // services.AddHostedService<OrderNotificationConsumer>();

            return services;
        }
    }
}
