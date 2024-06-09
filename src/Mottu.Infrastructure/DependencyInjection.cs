using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mottu.Application.Deliverymen.Commands;
using Mottu.Application.Interfaces;
using Mottu.Application.Motorcycles.Commands;
using Mottu.Application.Motorcycles.Queries;
using Mottu.Infrastructure.Repositories;
using System.Reflection;

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

            return services;
        }
    }
}
