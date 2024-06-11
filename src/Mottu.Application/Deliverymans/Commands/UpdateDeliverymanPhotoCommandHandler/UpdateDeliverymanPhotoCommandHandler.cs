using MediatR;
using Mottu.Infrastructure.Repositories;
using Mottu.Application.Services;
using Mottu.Application.Deliverymen.Queries;

namespace Mottu.Application.Deliverymen.Commands
{
    public class UpdateDeliverymanPhotoCommandHandler : IRequestHandler<UpdateDeliverymanPhotoCommand, string>
    {
        private readonly IDeliverymanRepository _deliverymanRepository;
        private readonly S3FileService _s3FileService;
        private readonly IMediator _mediator;

        public UpdateDeliverymanPhotoCommandHandler
        (
          IDeliverymanRepository deliverymanRepository, 
          S3FileService s3FileService,
          IMediator mediator)
        {
            _deliverymanRepository = deliverymanRepository;
            _s3FileService = s3FileService;
            _mediator = mediator;
        }

        public async Task<string> Handle(UpdateDeliverymanPhotoCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.UserId) || !Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new Exception("Invalid user identity.");
            }

            var query = new GetDeliverymanIdByUserIdQuery(userId);

            var deliverymanId = await _mediator.Send(query, cancellationToken);

            if (deliverymanId == null)
            {
                throw new Exception("Delivery person not found.");
            }

            if (request.Photo == null || request.Photo.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            using (var stream = new MemoryStream())
            {
                await request.Photo.CopyToAsync(stream);
                stream.Position = 0;
                var fileUrl = await _s3FileService.UploadFileAsync(stream, request.Photo.FileName);

                await _deliverymanRepository.UpdateDeliverymanPhotoUrlAsync(new Guid(deliverymanId.ToString()), fileUrl);

                return fileUrl;
            }
        }
    }
}
