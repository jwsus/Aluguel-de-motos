using Mottu.Application.Rentals.Commands;
using Mottu.Domain.Entities;
using System;

namespace Mottu.Application.Common.Interfaces
{
    public interface IRentalCostStrategy
    {
        ReturnRentalResult CalculateCosts(Rental rental, DateTime returnDate);
    }
}
