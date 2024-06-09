using MediatR;
using Mottu.Application.Interfaces;
using Mottu.Application.Motorcycles.CreateMotorcycle.Commands;
using Mottu.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Motorcycles.Commands
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, Guid>
    {
        private readonly IMotorcycleRepository _repository;

        public CreateMotorcycleCommandHandler(IMotorcycleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            // Verificar se a placa já existe
            if (await _repository.LicensePlateExistsAsync(request.LicensePlate))
            {
                throw new ArgumentException("A motorcycle with this license plate already exists.");
            }

            var motorcycle = new Motorcycle
            { 
                Id = Guid.NewGuid(),
                Year = request.Year,
                Model = request.Model,
                LicensePlate = request.LicensePlate
            };

            return await _repository.AddAsync(motorcycle);
        }
    }
}
