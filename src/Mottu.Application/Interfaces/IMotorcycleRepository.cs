using Mottu.Domain.Entities;
using System.Threading.Tasks;

namespace Mottu.Application.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task<bool> LicensePlateExistsAsync(string licensePlate);
        Task<Guid> AddAsync(Motorcycle motorcycle);
        Task UpdatePlateAsync(Guid id, string newPlate);
    }
}
