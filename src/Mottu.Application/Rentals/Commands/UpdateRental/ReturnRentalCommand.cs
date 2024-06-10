using MediatR;
using System;

namespace Mottu.Application.Rentals.Commands
{
    public class ReturnRentalCommand : IRequest<ReturnRentalResult>
    {
        public Guid RentalId { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    public class ReturnRentalResult
    {
        public decimal TotalCost { get; set; }
        public decimal AdditionalCharges { get; set; }
        public decimal PenaltyCharges { get; set; }
    }
}
