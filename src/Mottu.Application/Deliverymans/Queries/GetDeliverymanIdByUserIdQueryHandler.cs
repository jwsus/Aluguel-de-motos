using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Deliverymen.Queries
{
    public record GetDeliverymanIdByUserIdQuery(Guid UserId) : IRequest<Guid>;
    public class GetDeliverymanIdByUserIdQueryHandler : IRequestHandler<GetDeliverymanIdByUserIdQuery, Guid>
    {
        private readonly IApplicationDbContext _context;

        public GetDeliverymanIdByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(GetDeliverymanIdByUserIdQuery request, CancellationToken cancellationToken)
        {
            var deliverymanId = await _context.Deliverymans
                                              .Where(d => d.UserId == request.UserId)
                                              .Select(d => d.Id)
                                              .FirstOrDefaultAsync(cancellationToken);

            if (deliverymanId == default)
            {
                throw new Exception("Deliveryman not found.");
            }

            return deliverymanId;
        }
    }
}
