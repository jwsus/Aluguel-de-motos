using Mottu.Domain.Entities;

public class Order : BaseModel
{
    public Guid Id { get; set; }
    public decimal RideValue { get; set; }
    public OrderSituation Situation { get; set; } 
    public Guid? DeliverymanId { get; set; } 
    public Deliveryman Deliveryman { get; set; } 
}
