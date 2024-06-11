using Mottu.Domain.Entities;
using Mottu.Infrastructure.Data;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

  public async Task AddNotificationsAsync(IEnumerable<Notification> notifications)
  {
      _context.Notifications.AddRange(notifications);
      await _context.SaveChangesAsync();
  }
  
}
