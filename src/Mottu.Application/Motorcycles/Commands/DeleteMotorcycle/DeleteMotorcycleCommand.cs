using MediatR;
using System;

namespace Mottu.Application.Motorcycles.Commands
{
    public class DeleteMotorcycleCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
 