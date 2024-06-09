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

        // Foreign key to User
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
