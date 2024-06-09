using MediatR;
using Mottu.Domain.Entities;

namespace Mottu.Application.Rentals.Commands
{
    public class CreateRentalCommand : IRequest<Rental>
    {
        public Guid MotorcycleId { get; set; }
        public Guid DeliverymanId { get; set; }
        public RentalPlan Plan { get; set; }
    }
}
