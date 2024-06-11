using Mottu.Domain.Entities;

public class Order : BaseModel
{
    public Guid Id { get; set; }
    public decimal RideValue { get; set; }
    public OrderSituation Situation { get; set; } 
    public Guid? DeliverymanId { get; set; } // Nullable to allow for orders that haven't been accepted yet
    public Deliveryman Deliveryman { get; set; } // Navigation property
}
