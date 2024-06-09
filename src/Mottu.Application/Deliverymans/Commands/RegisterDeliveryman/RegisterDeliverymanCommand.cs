using MediatR;
using Mottu.Domain.Entities;

namespace Mottu.Application.Deliverymen.Commands
{
    public class RegisterDeliverymanCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public DateTime BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; }
        public LicenseType LicenseType { get; set; }
        public string LicenseImagePath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
