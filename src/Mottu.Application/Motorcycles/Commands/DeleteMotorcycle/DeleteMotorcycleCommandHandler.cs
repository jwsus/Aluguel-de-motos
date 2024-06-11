using MediatR;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Interfaces;
using Mottu.Application.Motorcycles.Queries;

namespace Mottu.Application.Motorcycles.Commands
{
    public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMediator _mediator;

        public DeleteMotorcycleCommandHandler
        (
            IApplicationDbContext context, 
            IMotorcycleRepository motorcycleRepository,
            IMediator mediator
        )
        {
            _context = context;
            _motorcycleRepository = motorcycleRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = new Motorcycle { Id = request.Id };

            var query = new CheckMotorcycleHasRentalsQuery(request.Id);
            var motorcycleUsed = await _mediator.Send(query, cancellationToken);

            if (motorcycleUsed)
            {
                throw new InvalidOperationException("Motorcycle cannot be deleted as it has already been rented.");
            }

            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
