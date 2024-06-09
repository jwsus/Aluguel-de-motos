namespace Mottu.Domain.Entities
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;

        public void UpdateuUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDeletedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
