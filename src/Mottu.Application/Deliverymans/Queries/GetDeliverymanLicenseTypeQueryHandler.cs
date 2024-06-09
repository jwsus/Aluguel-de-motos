using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Deliverymen.Queries
{
    
    public record GetDeliverymanLicenseTypeQuery(Guid Id) : IRequest<DeliverymanLicenseTypeDto>;
   
    public class DeliverymanLicenseTypeDto
    {
        public Guid Id { get; set; }
        public LicenseType LicenseType { get; set; }
    }
    public class GetDeliverymanLicenseTypeQueryHandler : IRequestHandler<GetDeliverymanLicenseTypeQuery, DeliverymanLicenseTypeDto>
    {
      
        private readonly IApplicationDbContext _context;

        public GetDeliverymanLicenseTypeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeliverymanLicenseTypeDto> Handle(GetDeliverymanLicenseTypeQuery request, CancellationToken cancellationToken)
        {
            return await _context.Deliverymans
                                 .Where(d => d.UserId == request.Id)
                                 .Select(d => new DeliverymanLicenseTypeDto
                                 {
                                     Id = d.Id,
                                     LicenseType = d.LicenseType
                                 })
                                 .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
