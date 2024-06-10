public class Order
{
    public Guid Id { get; set; }
    public decimal RideValue { get; set; }
    public OrderSituation Situation { get; set; } 
}
