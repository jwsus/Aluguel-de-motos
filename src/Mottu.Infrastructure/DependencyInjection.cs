using MediatR;
using Microsoft.Extensions.DependencyInjection;
// using Mottu.Api.Services;

// using Mottu.Api.Services;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Deliverymen.Commands;
using Mottu.Application.Interfaces;
using Mottu.Application.Motorcycles.Commands;
using Mottu.Application.Motorcycles.Queries;
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

            // Registro do repositório (se o repositório estiver em outro projeto, certifique-se de referenciá-lo)
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeliverymanRepository, DeliverymanRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            // services.AddSingleton<OrderNotificationProducer>(); // Registrar o serviço como Singleton
            // services.AddHostedService<OrderNotificationConsumer>();

            return services;
        }
    }
}
