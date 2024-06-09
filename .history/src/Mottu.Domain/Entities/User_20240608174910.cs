namespace Mottu.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; } // Role can be "Admin" or "Deliveryman"
    }
}
