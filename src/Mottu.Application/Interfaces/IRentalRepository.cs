using Mottu.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Mottu.Application.Common.Interfaces
{
    public interface IRentalRepository
    {
        Task<Guid> AddAsync(Rental rental);
        Task UpdateRentalCostAsync(Guid rentalId, DateTime endDate, decimal totalCost, CancellationToken cancellationToken);
    }
}
