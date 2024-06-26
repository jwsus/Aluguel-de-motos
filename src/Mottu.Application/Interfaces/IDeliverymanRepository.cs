using Mottu.Domain.Entities;
using System.Threading.Tasks;

namespace Mottu.Infrastructure.Repositories
{
    public interface IDeliverymanRepository
    {
        Task<Deliveryman> GetDeliverymanByUserIdAsync(Guid userId);
        Task AddDeliverymanAsync(Deliveryman deliveryman);
        Task UpdateDeliverymanPhotoUrlAsync(Guid deliverymanId, string photoUrl);
    }
}
