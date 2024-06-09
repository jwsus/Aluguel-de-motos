using MediatR;
using System;

namespace Mottu.Application.Motorcycles.Commands
{
    public class UpdateMotorcyclePlateCommand : IRequest
    {
        public Guid Id { get; set; } 
        public string NewPlate { get; set; } 
    }
}