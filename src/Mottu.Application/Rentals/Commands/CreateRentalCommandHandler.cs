using MediatR;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Deliverymen.Queries;
using Mottu.Application.Motorcycles.Queries;
using Mottu.Application.Rentals.Commands;
using Mottu.Domain.Entities;

namespace Mottu.Application.Rentals.Handlers
{
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Rental>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CreateRentalCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Rental> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deliverymanId = await ValidateDeliveryman(request.DeliverymanId);
                await ValidateMotorcycle(request.MotorcycleId);

                var startDate = DateTime.Now.AddDays(1);

                DateTime predictedEndDate;

                var (durationInDays, dailyRate) = GetRentalPlanDetails(request.Plan);

                predictedEndDate = startDate.AddDays(durationInDays).ToUtc();

                var rental = CreateRental(request, startDate, predictedEndDate, dailyRate * durationInDays, deliverymanId);

                
                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync(cancellationToken);

                return rental;
            }
            catch (Exception ex)
            {
                throw;
            } 
        }

        private async Task<Guid> ValidateDeliveryman(Guid deliverymanId)
        {
            var deliveryman = await _mediator.Send(new GetDeliverymanLicenseTypeQuery(deliverymanId));

            if (deliveryman == null)
            {
                throw new InvalidOperationException("Deliveryman not found.");
            }

            if (deliveryman.LicenseType != LicenseType.A)
            {
                throw new InvalidOperationException("Only delivery men with A license can rent motorcycles.");
            }

            return deliveryman.Id;
        }

        private async Task ValidateMotorcycle(Guid motorcycleId)
        {
            var motorcycleExists = await _mediator.Send(new CheckMotorcycleExistsQuery(motorcycleId));

            if (!motorcycleExists)
            {
                throw new InvalidOperationException("Motorcycle not found.");
            }

            var isMotorcycleAvailable = await _mediator.Send(new CheckMotorcycleAvailableQuery(motorcycleId));

            if (!isMotorcycleAvailable)
            {
                throw new InvalidOperationException("Motorcycle not available.");
            }
        }

        public (int DurationInDays, decimal DailyRate) GetRentalPlanDetails(RentalPlan plan)
        {
            //TODO: atualizar valores nos atributos do enum
            return plan switch
            {
                RentalPlan.SevenDays => (7, 30m),
                RentalPlan.FifteenDays => (15, 28m),
                RentalPlan.ThirtyDays => (30, 22m),
                _ => throw new InvalidOperationException("Invalid rental plan."),
            };
        }

         public Rental CreateRental(CreateRentalCommand request, DateTime startDate, DateTime predictedEndDate, decimal totalCost, Guid deliverymanId)
        {
            return new Rental
            {
                MotorcycleId = request.MotorcycleId,
                DeliverymanId = deliverymanId,
                StartDate = startDate.ToUtc(),
                EndDate = null,
                PredictedEndDate = predictedEndDate.ToUtc(),
                TotalCost = totalCost,
                Plan = request.Plan
            };
        }
    }
}
