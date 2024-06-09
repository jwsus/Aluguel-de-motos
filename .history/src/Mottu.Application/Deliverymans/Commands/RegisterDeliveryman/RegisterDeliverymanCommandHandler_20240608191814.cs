using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Repositories;

namespace Mottu.Application.Deliverymen.Commands
{
    public class RegisterDeliverymanCommandHandler : IRequestHandler<RegisterDeliverymanCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeliverymanRepository _deliverymanRepository;

        public RegisterDeliverymanCommandHandler(IUserRepository userRepository, IDeliverymanRepository deliverymanRepository)
        {
            _userRepository = userRepository;
            _deliverymanRepository = deliverymanRepository;
        }

        public async Task<Guid> Handle(RegisterDeliverymanCommand request, CancellationToken cancellationToken)
        {
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
