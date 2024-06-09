using MediatR;

namespace Mottu.Application.Motorcycles.CreateMotorcycle.Commands
{
    public class CreateMotorcycleCommand : IRequest<Guid>
    {
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }
}
