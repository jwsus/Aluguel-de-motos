namespace Mottu.Domain.Entities
{
    public class Notification : BaseModel
    {
        public Guid DeliverymanId { get; set; }
        public Guid OrderId { get; set; }
        public string Message { get; set; }
    }
}
