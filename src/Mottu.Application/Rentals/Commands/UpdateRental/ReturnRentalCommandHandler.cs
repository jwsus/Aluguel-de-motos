using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Rentals.Commands;
using Mottu.Application.Rentals.Strategies;
using Mottu.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Rentals.Handlers
{
    public class ReturnRentalCommandHandler : IRequestHandler<ReturnRentalCommand, ReturnRentalResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ReturnRentalCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IRentalRepository _rentalRepository;

        public ReturnRentalCommandHandler
        (
            IApplicationDbContext context, 
            ILogger<ReturnRentalCommandHandler> logger,
            IMediator mediator,
            IRentalRepository rentalRepository
        )
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _rentalRepository = rentalRepository;
        }

        public async Task<ReturnRentalResult> Handle(ReturnRentalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var rental = await _mediator.Send(new GetRentalByIdQuery(request.RentalId));
                if (rental == null)
                {
                    throw new InvalidOperationException("Rental not found.");
                }

                if (rental.EndDate != null)
                {
                    throw new InvalidOperationException("Rental has already been returned.");
                }

                IRentalCostStrategy costStrategy = GetCostStrategy(rental, request.ReturnDate);
                var result = costStrategy.CalculateCosts(rental, request.ReturnDate);

                rental.EndDate = request.ReturnDate.ToUtc();
                rental.TotalCost = result.TotalCost;

                await _rentalRepository.UpdateRentalCostAsync(request.RentalId, request.ReturnDate, result.TotalCost, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while returning rental.");
                throw;
            }

        }

        private IRentalCostStrategy GetCostStrategy(Rental rental, DateTime returnDate)
        {
            if (returnDate.Date < rental.PredictedEndDate.Date)
            {
                return new EarlyReturnCostStrategy();
            }
            else if (returnDate.Date > rental.PredictedEndDate.Date)
            {
                return new LateReturnCostStrategy();
            }
            else
            {
                return new OnTimeReturnCostStrategy();
            }
        }
    }
}
