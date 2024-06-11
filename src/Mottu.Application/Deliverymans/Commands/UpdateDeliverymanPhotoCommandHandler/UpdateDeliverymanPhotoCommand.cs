using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace Mottu.Application.Deliverymen.Commands
{
    public class UpdateDeliverymanPhotoCommand : IRequest<string>
    {
        public string UserId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
