namespace Mottu.Domain.Entities
{
    public class Deliveryman : BaseModel
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public DateTime BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; }
        public LicenseType LicenseType { get; set; } // A, B, or A+B
        public string LicenseImagePath { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
