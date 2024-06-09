namespace Mottu.Domain.Entities
{
    public class Rental : BaseModel
    {
        public Guid MotorcycleId { get; set; }
        public Guid DeliverymanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime PredictedEndDate { get; set; }
        public decimal TotalCost { get; set; }
        public RentalPlan Plan { get; set; }

        public virtual Motorcycle Motorcycle { get; set; }
        public virtual Deliveryman Deliveryman { get; set; }
    }
}
