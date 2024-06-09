using MediatR;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Interfaces;

namespace Mottu.Application.Motorcycles.Commands
{
    public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public DeleteMotorcycleCommandHandler(IApplicationDbContext context, IMotorcycleRepository motorcycleRepository)
        {
            _context = context;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = new Motorcycle { Id = request.Id };

            // Verifique se existem locações aqui quando implementar essa parte
            // Por enquanto, vamos assumir que a verificação de locações será feita posteriormente

            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
