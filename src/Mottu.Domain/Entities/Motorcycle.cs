namespace Mottu.Domain.Entities
{
    public class Motorcycle : BaseModel
    {
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }
}
