using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Interfaces;
using Mottu.Domain.Entities;

namespace Mottu.Application.Motorcycles.Commands
{
    public class UpdateMotorcyclePlateCommandHandler : IRequestHandler<UpdateMotorcyclePlateCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public UpdateMotorcyclePlateCommandHandler
        (
            IApplicationDbContext context,
            IMotorcycleRepository motorcycleRepository
        )
        {
            _context = context;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<Unit> Handle(UpdateMotorcyclePlateCommand request, CancellationToken cancellationToken)
        {
           var existingMotorcycle = await _context.Motorcycles
                                        .AnyAsync(m => m.LicensePlate == request.NewPlate, cancellationToken);
            if (existingMotorcycle)
            {
                throw new InvalidOperationException("A motorcycle with the same plate already exists.");
            }
            
            await _motorcycleRepository.UpdatePlateAsync(request.Id, request.NewPlate);
            return Unit.Value;
        }
    }
}
