using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;

namespace Mottu.Application.Deliverymen.Queries
{

    public class GetDeliverymanByUniqueFieldsQuery : IRequest<Deliveryman>
    {
        public string Cnpj { get; set; }
        public string DriverLicenseNumber { get; set; }
    }
    public class GetDeliverymanByUniqueFieldsQueryHandler : IRequestHandler<GetDeliverymanByUniqueFieldsQuery, Deliveryman>
    {
        private readonly IApplicationDbContext _context;

        public GetDeliverymanByUniqueFieldsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Deliveryman> Handle(GetDeliverymanByUniqueFieldsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Deliverymans
                .FirstOrDefaultAsync(d => d.Cnpj == request.Cnpj || d.DriverLicenseNumber == request.DriverLicenseNumber, cancellationToken);
        }
    }
}
