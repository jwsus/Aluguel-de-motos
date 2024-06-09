using MediatR;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Rentals.Commands;
using Mottu.Domain.Entities;

namespace Mottu.Application.Rentals.Handlers
{
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Rental>
    {
        private readonly IApplicationDbContext _context;

        public CreateRentalCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rental> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var deliveryman = await _context.Deliverymans.FindAsync(request.DeliverymanId);
            if (deliveryman == null || deliveryman.LicenseType != LicenseType.A)
            {
                throw new InvalidOperationException("Only delivery men with A license can rent motorcycles.");
            }

            var motorcycle = await _context.Motorcycles.FindAsync(request.MotorcycleId);
            if (motorcycle == null)
            {
                throw new InvalidOperationException("Motorcycle not found.");
            }

            var startDate = DateTime.Now.AddDays(1);
            DateTime endDate;
            decimal dailyRate;

            switch (request.Plan)
            {
                case RentalPlan.SevenDays:
                    endDate = startDate.AddDays(7);
                    dailyRate = 30m;
                    break;
                case RentalPlan.FifteenDays:
                    endDate = startDate.AddDays(15);
                    dailyRate = 28m;
                    break;
                case RentalPlan.ThirtyDays:
                    endDate = startDate.AddDays(30);
                    dailyRate = 22m;
                    break;
                default:
                    throw new InvalidOperationException("Invalid rental plan.");
            }

            var rental = new Rental
            {
                MotorcycleId = request.MotorcycleId,
                DeliverymanId = request.DeliverymanId,
                StartDate = startDate,
                EndDate = endDate,
                PredictedEndDate = endDate,
                TotalCost = dailyRate * (endDate - startDate).Days,
                Plan = request.Plan
            };

            // _context.Rentals.Add(rental);
            // await _context.SaveChangesAsync(cancellationToken);

            return rental;
        }
    }
}
