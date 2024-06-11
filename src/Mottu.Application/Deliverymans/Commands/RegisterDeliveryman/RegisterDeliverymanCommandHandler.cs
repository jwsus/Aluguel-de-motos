using System.Security.Cryptography;
using System.Text;
using MediatR;
using Mottu.Application.Deliverymen.Queries;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Repositories;

namespace Mottu.Application.Deliverymen.Commands
{
    public class RegisterDeliverymanCommandHandler : IRequestHandler<RegisterDeliverymanCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeliverymanRepository _deliverymanRepository;
        private readonly IMediator _mediator;

        public RegisterDeliverymanCommandHandler
        (
            IUserRepository userRepository, 
            IDeliverymanRepository deliverymanRepository,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _deliverymanRepository = deliverymanRepository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(RegisterDeliverymanCommand request, CancellationToken cancellationToken)
        {
            var query = new GetDeliverymanByUniqueFieldsQuery
            {
                Cnpj = request.Cnpj,
                DriverLicenseNumber = request.DriverLicenseNumber
            };
            var existingDeliveryman = await _mediator.Send(query, cancellationToken);

            if (existingDeliveryman != null)
            {
                if (existingDeliveryman.Cnpj == request.Cnpj)
                {
                    throw new Exception("A deliveryman with this CNPJ already exists.");
                }
                if (existingDeliveryman.DriverLicenseNumber == request.DriverLicenseNumber)
                {
                    throw new Exception("A deliveryman with this Driver License Number already exists.");
                }
            }

            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = CreatePasswordHash(request.Password),
                Role = UserRole.Deliveryman
            };

            await _userRepository.AddUserAsync(user);

            var deliveryman = new Deliveryman
            {
                Name = request.Name,
                Cnpj = request.Cnpj,
                BirthDate = request.BirthDate,
                DriverLicenseNumber = request.DriverLicenseNumber,
                LicenseType = request.LicenseType,
                LicenseImagePath = request.LicenseImagePath,
                UserId = user.Id
            };

            await _deliverymanRepository.AddDeliverymanAsync(deliveryman);

            return deliveryman.Id;
        }

        private string CreatePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
